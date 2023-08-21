using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.Factories.Interfaces;
using TestTask.BankAccountManagement.Helpers.Interfaces;
using TestTask.BankAccountManagement.Models;
using TestTask.BankAccountManagement.Shared.Models.Dtos;

namespace TestTask.BankAccountManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly IUserHelper _userHelper;

        public AuthController(
            IManagerService managerService,
            IJwtTokenFactory jwtTokenFactory,
            IUserHelper userHelper)
        {
            _managerService = managerService;
            _jwtTokenFactory = jwtTokenFactory;
            _userHelper = userHelper;
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErrorDetailsDto), 500)]
        [ProducesResponseType(typeof(ErrorDetailsDto), 401)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var manager = await _managerService.LoginAsync(loginRequest.Login, loginRequest.Pin);
            var token = _jwtTokenFactory.Create(manager.Login, manager.Id);
            var response = new LoginResponse
            {
                Token = token
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("user-info")]
        [ProducesResponseType(typeof(ManagerDto), 200)]
        [ProducesResponseType(typeof(ErrorDetailsDto), 500)]
        [ProducesResponseType(typeof(ErrorDetailsDto), 401)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var currentUserId = _userHelper.GetCurrentUserId();
            var manager = await _managerService.GetAsync(currentUserId);

            return Ok(manager);
        }
    }
}
