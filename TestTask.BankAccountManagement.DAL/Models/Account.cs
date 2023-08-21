using TestTask.BankAccountManagement.DAL.Models.Enums;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class Account : SoftDeletableEntity
    {
        public decimal Balance { get; set; }

        public AccountType Type { get; set; }

        public string Iban { get; set; }

        public long CountryId { get; set; }

        public Country Country { get; set; }

        public bool IsHardBlocked { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? BlockedAt { get; set; }

        public long CreatedByManagerId { get; set; }

        public Manager CreatedByManager { get; set; }

        public List<AccountOperation> Operations { get; set; }
        
        public List<Transaction> CreditTransactions { get; set; }
        
        public List<Transaction> DebitTransactions { get; set; }
    }
}
