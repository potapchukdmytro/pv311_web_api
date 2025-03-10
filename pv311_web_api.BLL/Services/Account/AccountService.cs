using AutoMapper;
using Microsoft.AspNetCore.Identity;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.DTOs.User;
using pv311_web_api.BLL.Services.Email;
using pv311_web_api.DAL.Entities;
using System.Text;

namespace pv311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private IEmailService _emailService;
        private IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<bool> EmailConfirmAsync(string id, string base64)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return false;
            }

            var bytes = Convert.FromBase64String(base64);
            var token = Encoding.UTF8.GetString(bytes);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
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

        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
        {
            if(await _userManager.FindByEmailAsync(dto.Email) != null)
                return null;
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return null;

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(result.Succeeded && await _roleManager.RoleExistsAsync("user"))
            {
                result = await _userManager.AddToRoleAsync(user, "user");
            }

            if (!result.Succeeded)
                return null;

            await SendEmailConfirmAsync(user.Id);

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<bool> SendEmailConfirmAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var bytes = Encoding.UTF8.GetBytes(token);
                var base64 = Convert.ToBase64String(bytes);

                string messageBody = $"<a href='https://localhost:7220/api/account/emailConfirm?id={user.Id}&t={base64}'>Підтвердити пошту</a>";

                await _emailService.SendMailAsync("dmytro.potapchuk22@gmail.com", "Email confirm", messageBody, true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
    }
}
