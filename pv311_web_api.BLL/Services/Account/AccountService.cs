using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.Services.Email;
using pv311_web_api.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pv311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, RoleManager<AppRole> roleManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
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

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            AppUser? user = null;

            if (dto.Login.Contains('@'))
                user = await _userManager.FindByEmailAsync(dto.Login);
            else
                user = await _userManager.FindByNameAsync(dto.Login);

            if (user == null)
                return new ServiceResponse($"Користувача з логіном '${dto.Login}' не знайдено");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
            {
                return new ServiceResponse($"Пароль вказано невірно");
            }

            // Jwt generate
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? ""),
                new Claim("image", user.Image ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim("role", r));
                claims.AddRange(roleClaims);
            }

            string? audience = _configuration["JwtSettings:Audience"];
            string? issuer = _configuration["JwtSettings:Issuer"];
            string? secretKey = _configuration["JwtSettings:SecretKey"];
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpMinutes"] ?? "");

            if(audience == null || issuer == null || secretKey == null)
            {
                throw new ArgumentNullException("Jwt settings not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtHandler = new JwtSecurityTokenHandler();
            string jwtToken = jwtHandler.WriteToken(token);
            return new ServiceResponse("Успішний вхід", true, jwtToken);
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        {
            if(await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse($"Пошта '{dto.Email}' вже використовується");
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse($"Ім'я користувача '{dto.UserName}' вже використовується");

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }
            
            if(result.Succeeded && await _roleManager.RoleExistsAsync("user"))
            {
                result = await _userManager.AddToRoleAsync(user, "user");
            }

            if (!result.Succeeded)
            {
                return new ServiceResponse(result.Errors.First().Description);
            }

            await SendEmailConfirmAsync(user.Id);

            // Jwt generate
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? ""),
                new Claim("image", user.Image ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim("role", r));
                claims.AddRange(roleClaims);
            }

            string? audience = _configuration["JwtSettings:Audience"];
            string? issuer = _configuration["JwtSettings:Issuer"];
            string? secretKey = _configuration["JwtSettings:SecretKey"];
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpMinutes"] ?? "");

            if (audience == null || issuer == null || secretKey == null)
            {
                throw new ArgumentNullException("Jwt settings not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtHandler = new JwtSecurityTokenHandler();
            string jwtToken = jwtHandler.WriteToken(token);
            return new ServiceResponse("Успішна реєстрація", true, jwtToken);
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
