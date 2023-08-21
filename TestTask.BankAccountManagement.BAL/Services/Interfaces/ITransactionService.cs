using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateAsync(CreateTransactionRequest request, long currentManagerId);
    }
}
