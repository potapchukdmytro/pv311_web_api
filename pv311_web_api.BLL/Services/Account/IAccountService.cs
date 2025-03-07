﻿using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<AppUser?> LoginAsync(LoginDto dto);
        Task<AppUser?> RegisterAsync(RegisterDto dto);
        Task<bool> EmailConfirmAsync(string id, string token);
    }
}
