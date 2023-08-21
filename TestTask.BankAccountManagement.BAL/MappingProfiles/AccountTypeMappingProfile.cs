using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.BAL.MappingProfiles
{
    public class AccountTypeMappingProfile : Profile
    {
        private const string AccountTypeNotSupportedExceptionMessage = "Account Type {0} not supported";

        public AccountTypeMappingProfile()
        {
            CreateMap<AccountTypeDto, AccountType>().ConvertUsing((value, _) =>
            {
                return value switch
                {
                    AccountTypeDto.Current => AccountType.Current,
                    AccountTypeDto.Savings => AccountType.Savings,
                    _ => throw new UserFriendlyMessageException(string.Format(AccountTypeNotSupportedExceptionMessage, value))
                };
            });
        }
    }
}
