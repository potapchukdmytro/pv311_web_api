using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Role;
using pv311_web_api.BLL.Services.Role;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id required");

            var role = await _roleService.GetByIdAsync(id);

            return role == null ? BadRequest("Role not found") : Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleDto dto)
        {
            var result = await _roleService.CreateAsync(dto);

            return result ? Ok("Role created") : BadRequest("Role not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleDto dto)
        {
            var result = await _roleService.UpdateAsync(dto);

            return result ? Ok("Role updated") : BadRequest("Role not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id required");

            var result = await _roleService.DeleteAsync(id);
            return result ? Ok("Role deleted") : BadRequest("Role not deleted");
        }
    }
}
