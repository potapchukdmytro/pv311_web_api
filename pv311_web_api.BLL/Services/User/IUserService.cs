using pv311_web_api.BLL.DTOs.User;

namespace pv311_web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
    }
}
