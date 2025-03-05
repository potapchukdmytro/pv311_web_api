using pv311_web_api.BLL.DTOs.Role;

namespace pv311_web_api.BLL.Services.Role
{
    public interface IRoleService
    {
        Task<bool> CreateAsync(RoleDto dto);
        Task<bool> UpdateAsync(RoleDto dto);
        Task<bool> DeleteAsync(string id);
        Task<RoleDto?> GetByIdAsync(string id);
        Task<IEnumerable<RoleDto>> GetAllAsync();
    }
}
