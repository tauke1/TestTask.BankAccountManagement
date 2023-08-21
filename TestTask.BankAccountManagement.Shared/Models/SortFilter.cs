using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.Shared.Models
{
    public class SortFilter
    {
        public string PropertyName { get; set; }
        public SortDirectionDto Direction { get; set; }
    }
}
