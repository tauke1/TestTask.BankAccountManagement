using AutoMapper;
using System.Data;
using System.Net;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.BAL.Wrappers.Interfaces;
using TestTask.BankAccountManagement.DAL;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Shared.Models.Dtos;
using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountOperationRepository _accountOperationRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountOperationService _accountOperationService;
        private readonly IRandomWrapper _randomWrapper;

        private const string AccountNotFoundException = "Account with Id {0} not found";
        private const string FromAccountHasInsufficientFundsExceptionMessage = "Source account with Id {0} have insufficient funds to make a transaction";


        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository, 
            IMapper mapper,
            IDateTimeWrapper dateTimeWrapper,
            IUnitOfWork unitOfWork,
            IAccountOperationService accountOperationService,
            IRandomWrapper randomWrapper,
            IAccountOperationRepository accountOperationRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _dateTimeWrapper = dateTimeWrapper;
            _unitOfWork = unitOfWork;
            _accountOperationService = accountOperationService;
            _randomWrapper = randomWrapper;
            _accountOperationRepository = accountOperationRepository;
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionRequest request, long currentManagerId)
        {
            await using var dbTransaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead);

            var fromAccount = await _accountRepository.SingleOrDefaultByIdAsync(request.FromAccountId);
            if (fromAccount == null)
            {
                var exceptionMessage = string.Format(AccountNotFoundException, request.FromAccountId);
                throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.UnprocessableEntity);
            }

            if (fromAccount.Balance - request.Amount < 0)
            {
                throw new UserFriendlyMessageException(string.Format(FromAccountHasInsufficientFundsExceptionMessage,
                    fromAccount.Id), HttpStatusCode.UnprocessableEntity);
            }

            var toAccount = await _accountRepository.SingleOrDefaultByIdAsync(request.ToAccountId);
            if (toAccount == null)
            {
                var exceptionMessage = string.Format(AccountNotFoundException, request.ToAccountId);
                throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.UnprocessableEntity);
            }

            await _accountOperationService.CheckOperationAllowedAsync(fromAccount);

            var now = _dateTimeWrapper.UtcNow;

            var transaction = new Transaction
            {
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                Amount = request.Amount,
                ReferenceRetrievalNumber = _randomWrapper.GenerateGuid().ToString(),
                Date = now,
                TriggeredByManagerId = currentManagerId
            };

            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            var creditAccountOperation = new AccountOperation
            {
                AccountId = fromAccount.Id,
                Amount = request.Amount,
                BalanceBefore = fromAccount.Balance,
                Date = now,
                TransactionId = transaction.Id,
                TransactionType = TransactionType.Credit
            };
            var debitAccountOperation = new AccountOperation
            {
                AccountId = toAccount.Id,
                Amount = request.Amount,
                BalanceBefore = toAccount.Balance,
                Date = now,
                TransactionId = transaction.Id,
                TransactionType = TransactionType.Debit
            };

            await _accountOperationRepository.AddRangeAsync(creditAccountOperation, debitAccountOperation);

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;
            _accountRepository.UpdateRange(fromAccount, toAccount);

            await _unitOfWork.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
