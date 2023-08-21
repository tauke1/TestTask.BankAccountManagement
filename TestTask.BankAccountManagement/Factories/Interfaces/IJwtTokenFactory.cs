namespace TestTask.BankAccountManagement.Factories.Interfaces
{
    public interface IJwtTokenFactory
    {
        string Create(string login, long userId);
    }
}
