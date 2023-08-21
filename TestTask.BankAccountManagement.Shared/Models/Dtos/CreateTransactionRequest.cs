using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class CreateTransactionRequest
    {
        public long FromAccountId { get; set; }

        public long ToAccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
