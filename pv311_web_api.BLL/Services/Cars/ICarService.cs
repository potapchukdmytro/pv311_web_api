using pv311_web_api.BLL.DTOs.Cars;

namespace pv311_web_api.BLL.Services.Cars
{
    public interface ICarService
    {
        Task<ServiceResponse> CreateAsync(CreateCarDto dto);
        Task<ServiceResponse> GetAllAsync();
    }
}
