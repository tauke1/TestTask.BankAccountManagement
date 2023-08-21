using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Enums;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class AccountOperation : IdentifiableEntity
    {
        public long AccountId { get; set; }

        public Account Account { get; set; }

        public decimal BalanceBefore { get; set; }

        public TransactionType TransactionType { get; set; }

        public long TransactionId { get; set; }

        public Transaction Transaction { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
