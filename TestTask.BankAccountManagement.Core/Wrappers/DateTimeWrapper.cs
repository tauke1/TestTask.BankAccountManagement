using TestTask.BankAccountManagement.Core.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.Core.Wrappers
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
