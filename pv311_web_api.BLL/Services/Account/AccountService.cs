﻿using Microsoft.AspNetCore.Identity;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> LoginAsync(LoginDto dto)
        {
            AppUser? user = null;

            if (dto.Login.Contains('@'))
                user = await _userManager.FindByEmailAsync(dto.Login);
            else
                user = await _userManager.FindByNameAsync(dto.Login);

            if (user == null)
                return null;

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            return result ? user : null;
        }
    }
}
