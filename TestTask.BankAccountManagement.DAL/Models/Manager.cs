using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class Manager : SoftDeletableEntity
    {
        public string Login { get; set; }

        public string PinHashed { get; set; }

        public ICollection<Transaction> TriggeredTransactions { get; set; }

        public ICollection<Account> CreatedAccounts { get; set; }
    }
}
