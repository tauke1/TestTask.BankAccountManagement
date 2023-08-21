using Microsoft.Extensions.Options;
using TestTask.BankAccountManagement.BAL.Models.Settings;
using TestTask.BankAccountManagement.BAL.SecurityHashers.Interfaces;

namespace TestTask.BankAccountManagement.BAL.SecurityHashers
{
    public class BCryptHasher : ISecurityHasher
    {
        private readonly BcryptSettings _settings;

        public BCryptHasher(IOptions<BcryptSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Hash(string key)
        {
            return BCrypt.Net.BCrypt.HashPassword(key, _settings.Rounds);
        }

        public bool Verify(string key, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(key, hashed);
        }
    }
}
