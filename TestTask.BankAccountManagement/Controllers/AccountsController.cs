using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.Helpers.Interfaces;
using TestTask.BankAccountManagement.Shared.Models;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserHelper _userHelper;

        public AccountsController(IAccountService accountService, IUserHelper userHelper)
        {
            _accountService = accountService;
            _userHelper = userHelper;
        }

        [ProducesResponseType(typeof(PagedList<AccountDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> SelectAsync([FromQuery] GetAccountsRequest request)
        {
            var currentUserId = _userHelper.GetCurrentUserId();
            return Ok(await _accountService.SelectAsync(request, currentUserId));
        }

        [ProducesResponseType(typeof(AccountDto), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await _accountService.GetAsync(id));
        }

        [ProducesResponseType(typeof(AccountDto), 200)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateAccountRequest request)
        {
            var currentUserId = _userHelper.GetCurrentUserId();
            return Ok(await _accountService.CreateAsync(request, currentUserId));
        }

        [ProducesResponseType(typeof(AccountDto), 200)]
        [HttpPatch("{id}/close")]
        public async Task<IActionResult> CloseAsync(long id)
        {
            return Ok(await _accountService.CloseAsync(id));
        }

        [ProducesResponseType(typeof(AccountDto), 200)]
        [HttpPatch("{id}/block")]
        public async Task<IActionResult> BlockAsync(long id, [FromBody] BlockAccountRequest request)
        {
            return Ok(await _accountService.BlockAsync(id, request.IsHardBlock));
        }

        [ProducesResponseType(typeof(AccountDto), 200)]
        [HttpPatch("{id}/unblock")]
        public async Task<IActionResult> UnblockAsync(long id)
        {
            return Ok(await _accountService.UnblockAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _accountService.DeleteAsync(id);
            return Ok();
        }
    }
}
