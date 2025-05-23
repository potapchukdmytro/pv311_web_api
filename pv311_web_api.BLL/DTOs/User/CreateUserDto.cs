﻿using Microsoft.AspNetCore.Http;
using pv311_web_api.BLL.DTOs.Role;

namespace pv311_web_api.BLL.DTOs.User
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? Image { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public List<string> Roles { get; set; } = [];
    }
}
