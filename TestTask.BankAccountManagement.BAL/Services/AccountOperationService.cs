using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Services
{
    public class AccountOperationService : IAccountOperationService
    {
        private readonly IAccountOperationRepository _accountOperationRepository;
        private readonly IAccountTypeSettingsRepository _accountTypeSettingsRepository;
        private readonly IDateTimeWrapper _dateTimeWrapper;

        private const string MaxCreditTransactionsPerDayCountExceedExceptionMessage =
            "Account with IBAN {0} reached maximum credit transactions per day count, can't make new transaction";
        private const string TransactionBlockedExceptionMessage =
            "Account with IBAN {0} is soft blocked, can't make new transaction";
        private const string TransactionHardBlockedExceptionMessage =
            "Account with IBAN {0} is hard blocked, can't make new transaction";
        private const string TransactionDeletedExceptionMessage =
            "Account with IBAN {0} is deleted, can't make new transaction";
        private const string AccountTypeSettingsNotFoundExceptionMessage =
            "Accoun type settings by account type {0} for account with Id {1} not found";

        public AccountOperationService(
            IAccountOperationRepository accountOperationRepository,
            IDateTimeWrapper dateTimeWrapper,
            IAccountTypeSettingsRepository accountTypeSettingsRepository)
        {
            _accountOperationRepository = accountOperationRepository;
            _dateTimeWrapper = dateTimeWrapper;
            _accountTypeSettingsRepository = accountTypeSettingsRepository;
        }

        public async Task CheckOperationAllowedAsync(Account account)
        {
            if (account.BlockedAt.HasValue)
            {
                var exceptionMessage = string.Format(account.IsHardBlocked ? TransactionHardBlockedExceptionMessage : TransactionBlockedExceptionMessage, account.Iban);

                throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.UnprocessableEntity);
            }

            if (account.DeletedAt.HasValue)
            {
                var exceptionMessage = string.Format(TransactionDeletedExceptionMessage, account.Iban);
                throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.UnprocessableEntity);
            }

            var accountTypeSettings = await _accountTypeSettingsRepository.SingleOrDefaultByFilterAsync(s => s.AccountType == account.Type);
            if (accountTypeSettings == null)
            {
                var exceptionMessage = string.Format(AccountTypeSettingsNotFoundExceptionMessage, account.Type, account.Id);
                throw new Exception(exceptionMessage);
            }

            var maxTransactionsPerDay = accountTypeSettings.MaxTransactionsPerDay;

            var today = _dateTimeWrapper.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            if (maxTransactionsPerDay.HasValue)
            {
                Expression<Func<AccountOperation, bool>> filterPredicate =
                    a => a.AccountId == account.Id && a.Date >= today && a.Date < tomorrow && a.TransactionType == TransactionType.Credit;
                var todayTransactionCount = await _accountOperationRepository.CountByFilterAsync(filterPredicate);

                if (todayTransactionCount >= maxTransactionsPerDay)
                {
                    var exceptionMessage =
                        string.Format(MaxCreditTransactionsPerDayCountExceedExceptionMessage, account.Iban);
                    throw new UserFriendlyMessageException(exceptionMessage, HttpStatusCode.UnprocessableEntity);
                }
            }
        }
    }
}
