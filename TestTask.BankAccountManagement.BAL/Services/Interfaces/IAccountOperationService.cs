using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Models.Enums;

namespace TestTask.BankAccountManagement.BAL.Services.Interfaces
{
    public interface IAccountOperationService
    {
        Task CheckOperationAllowedAsync(Account account);
    }
}
