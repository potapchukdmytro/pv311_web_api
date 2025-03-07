using Microsoft.AspNetCore.Identity;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.Services.Email;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private UserManager<AppUser> _userManager;
        private IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<bool> EmailConfirmAsync(string id, string token)
        {
            // тут має бути код підтвердження пошти
            return false;
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

        public async Task<AppUser?> RegisterAsync(RegisterDto dto)
        {
            //if (await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == dto.Email.ToUpper() || u.NormalizedUserName == dto.UserName.ToUpper()) != null)
            //{
            //    return null;
            //}

            if(await _userManager.FindByEmailAsync(dto.Email) != null)
                return null;
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return null;

            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return null;

            // send email message
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string messageBody = $"<a href='https://localhost:7220/api/account/emailConfirm?id={user.Id}&t={token}'>Підтвердити пошту</a>";

            await _emailService.SendMailAsync("dmytro.potapchuk22@gmail.com", "Email confirm", messageBody, true);

            return user;
        }
    }
}
