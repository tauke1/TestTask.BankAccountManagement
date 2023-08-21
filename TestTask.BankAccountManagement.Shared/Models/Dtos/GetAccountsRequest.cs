using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class GetAccountsRequest
    {
        private const int DefaultPageSize = 25;
        private const int DefaultPageNumber = 1;

        public string Search { get; set; }

        public long? CountryId { get; set; }

        public IEnumerable<AccountTypeDto> AccountTypes { get; set; }

        public bool? Closed { get; set; }

        public bool? Blocked { get; set; }

        public bool? CreatedByMe { get; set; }

        public PageFilter PageFilter { get; set; } = DefaultPageFilter;

        public SortFilter SortFilter { get; set; }

        private static PageFilter DefaultPageFilter = new PageFilter
        {
            Number = DefaultPageNumber,
            Size = DefaultPageSize
        };
    }
}
