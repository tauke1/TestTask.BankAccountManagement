using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Enums;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class Transaction : SoftDeletableEntity
    {
        public string Description { get; set; }

        public string ReferenceRetrievalNumber { get; set; }

        public decimal Amount { get; set; }

        public long FromAccountId { get; set; }

        public Account FromAccount { get; set; }

        public long ToAccountId { get; set; }

        public Account ToAccount { get; set; }

        public DateTime Date { get; set; }

        public long TriggeredByManagerId { get; set; }

        public Manager TriggeredByManager { get; set; }
    }
}
