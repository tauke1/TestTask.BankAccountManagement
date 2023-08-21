using AutoMapper;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.BAL.MappingProfiles
{
    public class TransactionTypeMappingProfile : Profile
    {
        private const string TransactionTypeNotSupportedExceptionMessage = "Transaction type {0} not supported";

        public TransactionTypeMappingProfile()
        {
            CreateMap<TransactionType, TransactionTypeDto>().ConvertUsing((value, _) =>
            {
                return value switch
                {
                    TransactionType.Debit => TransactionTypeDto.Debit,
                    TransactionType.Credit => TransactionTypeDto.Credit,
                    _ => throw new UserFriendlyMessageException(string.Format(TransactionTypeNotSupportedExceptionMessage, value))
                };
            });
        }
    }
}
