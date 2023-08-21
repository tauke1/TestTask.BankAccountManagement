namespace TestTask.BankAccountManagement.Models.Settings
{
    public class JwtSettings
    {

        public string Key { get; set; }

        public string Issuer { get; set; }

        public int ExpiresInSeconds { get; set; }
    }
}
