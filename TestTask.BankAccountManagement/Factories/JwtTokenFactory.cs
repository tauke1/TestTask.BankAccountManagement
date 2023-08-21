using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestTask.BankAccountManagement.Factories.Interfaces;
using TestTask.BankAccountManagement.Models.Settings;
using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.Factories
{
    public class JwtTokenFactory : IJwtTokenFactory
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTimeWrapper _dateTimeWrapper;

        public JwtTokenFactory(
            IOptions<JwtSettings> jwtSettingsOptions, 
            IDateTimeWrapper dateTimeWrapper)
        {
            _jwtSettings = jwtSettingsOptions.Value;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public string Create(string login, long userId)
        {
            var expiresInSeconds = _jwtSettings.ExpiresInSeconds;
            var issuer = _jwtSettings.Issuer;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, login),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: _dateTimeWrapper.UtcNow.AddSeconds(expiresInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
