using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.BLL.Services.Manufactures;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/manufacture")]
    public class ManufactureController : AppController
    {
        private readonly IManufactureService _manufactureService;

        public ManufactureController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateManufactureDto dto)
        {
            var result = await _manufactureService.CreateAsync(dto);
            return result ? Ok($"{dto.Name} created") : BadRequest("Error");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateManufactureDto dto)
        {
            var result = await _manufactureService.UpdateAsync(dto);
            return result ? Ok($"{dto.Name} updated") : BadRequest("Error");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if(!ValidateId(id, out string error))
            {
                return BadRequest(error);
            }

            var result = await _manufactureService.DeleteAsync(id);
            return result ? Ok($"Deleted") : BadRequest("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if(!ValidateId(id, out string error))
            {
                return BadRequest(error);
            }

            var result = await _manufactureService.GetByIdAsync(id);

            return result == null ? BadRequest("null") : Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _manufactureService.GetAllAsync();
            return CreateActionResult(response);
        }
    }
}
