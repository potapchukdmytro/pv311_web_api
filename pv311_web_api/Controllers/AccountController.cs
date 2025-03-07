﻿using FluentValidation;
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
        private readonly IValidator<LoginDto> _loginValidator;

        public AccountController(IAccountService accountService, IValidator<LoginDto> loginValidator)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var validResult = await _loginValidator.ValidateAsync(dto);

            if (!validResult.IsValid)
                return BadRequest(validResult);

            var user = await _accountService.LoginAsync(dto);

            return user == null ? BadRequest("Incorrect login or password") : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            return result == null ? BadRequest("Register error") : Ok(result);
        }

        [HttpGet("emailConfirm")]
        public async Task<IActionResult> EmailConfirmAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            var result = await _accountService.EmailConfirmAsync(id, t);
            
            if (!result)
                return NotFound();

            return Redirect("https://google.com");
        }
    }
}
