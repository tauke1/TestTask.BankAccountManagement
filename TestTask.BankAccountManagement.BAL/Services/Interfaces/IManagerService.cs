using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.Services.Interfaces
{
    public interface IManagerService
    {
        Task<ManagerDto> LoginAsync(string login, string pin);
        Task<ManagerDto> GetAsync(long id);
    }
}
