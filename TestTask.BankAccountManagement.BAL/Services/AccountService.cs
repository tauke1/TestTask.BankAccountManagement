using AutoMapper;
using System.Linq.Expressions;
using System.Net;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.BAL.Factories.Interfaces;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.DAL;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Shared.Models;
using TestTask.BankAccountManagement.Shared.Models.Dtos;
using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;
using TestTask.BankAccountManagement.BAL.Utilities.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly IIbanGeneratorFactory _ibanGeneratorFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IPredicateBuilder _predicateBuilder;

        private const string AccountNotFoundExceptionMessage = "Account with Id {0} not found";
        private const string AccountAlreadyBlockedExceptionMessage = "Account with Id {0} already blocked";
        private const string AccountAlreadyClosedExceptionMessage = "Account with Id {0} already closed";
        private const string CanNotUnblockNotBlockedAccountExceptionMessage = "Can not unblock account with Id {0} because it's not blocked";
        private const string CanNotUnblockHardBlockedAccountExceptionMessage = "Can not unblock account with Id {0}, because it's hard blocked";
        private const string CanNotUnblockClosedAccountExceptionMessage = "Can not unblock account with Id {0} because it's closed";
        private const string CanNotBlockClosedAccountExceptionMessage = "Can not block account with Id {0} because it's closed";
        private const string CountryNotFoundExceptionMessage = "Country with Id {0} not found";
        private const string TransactionStartingBalanceMustBeMoreThatZero = "Transaction starting balance must be more than zero";

        private static readonly HashSet<string> SortableProps = new ()
        {
            nameof(Account.Iban),
            nameof(Account.Balance),
            nameof(Account.OpenedAt),
            nameof(Account.ClosedAt),
            nameof(Account.BlockedAt)
        };

        public AccountService(
            IAccountRepository accountRepository, 
            IMapper mapper, 
            IIbanGeneratorFactory ibanGeneratorFactory, 
            ICountryRepository countryRepository, 
            IUnitOfWork unitOfWork,
            IDateTimeWrapper dateTimeWrapper,
            IPredicateBuilder predicateBuilder)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _ibanGeneratorFactory = ibanGeneratorFactory;
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
            _dateTimeWrapper = dateTimeWrapper;
            _predicateBuilder = predicateBuilder;
        }

        public async Task<PagedList<AccountDto>> SelectAsync(GetAccountsRequest request, long currentUserId)
        {
            var pageFilter = request.PageFilter;
            string sortByProp = null;
            var sortDirection = SortDirection.Ascending;
            if (request.SortFilter != null)
            {
                if (SortableProps.Contains(request.SortFilter.PropertyName))
                {
                    sortByProp = request.SortFilter.PropertyName;
                }

                sortDirection =_mapper.Map<SortDirection>(request.SortFilter.Direction);
            }

            var predicate = GetSelectPredicate(request, currentUserId);
            var accounts = await _accountRepository.SelectByFilterAndPaginationAsync(pageFilter.Number, pageFilter.Size,
                predicate, sortByProp, sortDirection);
            var count = await _accountRepository.CountByFilterAsync(predicate);

            return new PagedList<AccountDto>(
                        pageFilter.Size, 
                        pageFilter.Number, 
                        count,
                        accounts.Select(_mapper.Map<AccountDto>)
                    .ToList());
        }

        private Expression<Func<Account, bool>> GetSelectPredicate(GetAccountsRequest request, long currentUserId)
        {
            var accountTypes = request.AccountTypes?.Select(t => _mapper.Map<AccountType>(t)).ToList();
            Expression<Func<Account, bool>> accountTypesPredicate =
                accountTypes != null ? a => accountTypes.Contains(a.Type) : null;
            Expression<Func<Account, bool>> searchPredicate =
                request.Search != null ? a => a.Iban.Contains(request.Search) : null;
            Expression<Func<Account, bool>> countryPredicate =
                request.CountryId.HasValue ? a => a.CountryId == request.CountryId.Value : null;
            Expression<Func<Account, bool>> closedPredicate =
                request.Closed.HasValue ? a => a.ClosedAt.HasValue == request.Closed.Value : null;
            Expression<Func<Account, bool>> blockedPredicate =
                request.Blocked.HasValue ? a => a.BlockedAt.HasValue == request.Blocked.Value : null;
            Expression<Func<Account, bool>> createdByMePredicate =
                request.CreatedByMe.HasValue ? a => request.CreatedByMe.Value ? a.CreatedByManagerId == currentUserId : a.CreatedByManagerId != currentUserId : null;

            var finalPredicate = _predicateBuilder.And(
                accountTypesPredicate, 
                searchPredicate,
                countryPredicate,
                closedPredicate,
                blockedPredicate,
                createdByMePredicate);
            return finalPredicate;
        }

        public async Task<AccountDto> GetAsync(long id)
        {
            var account = await GetAccountInternalAsync(id);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> CreateAsync(CreateAccountRequest request, long currentUserId)
        {
            var country = await _countryRepository.SingleOrDefaultByIdAsync(request.CountryId);
            if (country == null)
            {
                throw new UserFriendlyMessageException(string.Format(CountryNotFoundExceptionMessage, request.CountryId));
            }

            if (request.StartingBalance < 0)
            {
                throw new UserFriendlyMessageException(TransactionStartingBalanceMustBeMoreThatZero,
                    HttpStatusCode.UnprocessableEntity);
            }

            var account = new Account
            {
                OpenedAt = _dateTimeWrapper.UtcNow,
                CountryId = country.Id,
                Type = _mapper.Map<AccountType>(request.Type),
                CreatedByManagerId = currentUserId,
                Balance = request.StartingBalance
            };

            var ibanGenerator = _ibanGeneratorFactory.Create();
            var iban = ibanGenerator.Generate(country.Code);
            account.Iban = iban.ToString();

            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> CloseAsync(long accountId)
        {
            var account = await GetAccountInternalAsync(accountId);
            if (account.BlockedAt.HasValue)
            {
                throw new UserFriendlyMessageException(string.Format(AccountAlreadyClosedExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            account.ClosedAt = _dateTimeWrapper.UtcNow;

            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> BlockAsync(long accountId, bool isHardBlock)
        {
            var account = await GetAccountInternalAsync(accountId);
            if (account.BlockedAt.HasValue)
            {
                throw new UserFriendlyMessageException(string.Format(AccountAlreadyBlockedExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            if (account.ClosedAt.HasValue)
            {
                throw new UserFriendlyMessageException(string.Format(CanNotBlockClosedAccountExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            account.IsHardBlocked = isHardBlock;
            account.BlockedAt = _dateTimeWrapper.UtcNow;

            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> UnblockAsync(long accountId)
        {
            var account = await GetAccountInternalAsync(accountId);
            if (!account.BlockedAt.HasValue)
            {
                throw new UserFriendlyMessageException(string.Format(CanNotUnblockNotBlockedAccountExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            if (account.IsHardBlocked)
            {
                throw new UserFriendlyMessageException(string.Format(CanNotUnblockHardBlockedAccountExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            if (account.ClosedAt.HasValue)
            {
                throw new UserFriendlyMessageException(string.Format(CanNotUnblockClosedAccountExceptionMessage, account.Id), HttpStatusCode.UnprocessableEntity);
            }

            account.BlockedAt = null;

            _accountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task DeleteAsync(long accountId)
        {
            var account = await GetAccountInternalAsync(accountId);

            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Account> GetAccountInternalAsync(long id)
        {
            var account = await _accountRepository.SingleOrDefaultByIdAsync(id);
            if (account == null)
            {
                var exceptionMessage = string.Format(AccountNotFoundExceptionMessage, id);
                throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.NotFound);
            }

            return account;
        }
    }
}
