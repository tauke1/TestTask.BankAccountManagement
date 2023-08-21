using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.Shared.Wrappers
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
