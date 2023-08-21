using TestTask.BankAccountManagement.Shared.Models;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.BAL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<PagedList<AccountDto>> SelectAsync(GetAccountsRequest request, long currentUserId);
        
        Task<AccountDto> GetAsync(long id);
        
        Task<AccountDto> CreateAsync(CreateAccountRequest request, long currentUserId);
        
        Task<AccountDto> CloseAsync(long accountId);

        Task<AccountDto> BlockAsync(long accountId, bool isHardBlock);

        Task<AccountDto> UnblockAsync(long accountId);

        Task DeleteAsync(long accountId);
    }
}
