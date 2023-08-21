using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestTask.BankAccountManagement.Helpers.Interfaces;

namespace TestTask.BankAccountManagement.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetCurrentUserId()
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var idStr = userPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;

            var idValue = long.Parse(idStr);
            return idValue;
        }
    }
}
