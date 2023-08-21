using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Enums;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class AccountTypeSettings : IdentifiableEntity
    {
        public AccountType AccountType { get; set; }

        public int? MaxTransactionsPerDay { get; set; }
    }
}
