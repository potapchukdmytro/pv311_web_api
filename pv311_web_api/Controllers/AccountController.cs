using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.Services.Account;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var user = await _accountService.LoginAsync(dto);

            return user == null ? BadRequest("Incorrect login or password") : Ok(user);
        }
    }
}
