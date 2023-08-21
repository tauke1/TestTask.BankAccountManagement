using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class AccountDto : BaseDto
    {
        public decimal Balance { get; set; }

        public AccountTypeDto Type { get; set; }

        public string Iban { get; set; }

        public bool IsHardBlocked { get; set; }

        public long CountryId { get; set; }

        public DateTime OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? BlockedAt { get; set; }
    }
}
