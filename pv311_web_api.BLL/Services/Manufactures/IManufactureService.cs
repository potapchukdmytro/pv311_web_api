using pv311_web_api.BLL.DTOs.Manufactures;

namespace pv311_web_api.BLL.Services.Manufactures
{
    public interface IManufactureService
    {
        Task<bool> CreateAsync(CreateManufactureDto dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(UpdateManufactureDto dto);
        Task<ManufactureDto?> GetByIdAsync(string id);
    }
}
