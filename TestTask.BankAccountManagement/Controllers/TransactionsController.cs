using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.Helpers.Interfaces;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserHelper _userHelper;

        public TransactionsController(ITransactionService transactionService, IUserHelper userHelper)
        {
            _transactionService = transactionService;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTransactionRequest request)
        {
            var currentManagerId = _userHelper.GetCurrentUserId();
            return Ok(await _transactionService.CreateAsync(request, currentManagerId));
        }
    }
}
