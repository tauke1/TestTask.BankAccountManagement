namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class TransactionDto
    {
        public long Id { get; set; }

        public decimal Amount { get; set; }

        public long FromAccountId { get; set; }

        public long ToAccountId { get; set; }
    }
}
