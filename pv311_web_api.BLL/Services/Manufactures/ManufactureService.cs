using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IImageService _imageService;

        public ManufactureService(IMapper mapper, AppDbContext context, IImageService imageService)
        {
            _mapper = mapper;
            _context = context;
            _imageService = imageService;
        }

        public async Task<bool> CreateAsync(CreateManufactureDto dto)
        {
            var entity = _mapper.Map<Manufacture>(dto);
            string? imageName = null;

            if (dto.Image != null)
            {
                imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (imageName != null)
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }
            }

            entity.Image = imageName;

            await _context.Manufactures.AddAsync(entity);
            var result = await _context.SaveChangesAsync();

            return result != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);
            if(entity == null)
            {
                return false;
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            _context.Manufactures.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<ManufactureDto?> GetByIdAsync(string id)
        {
            var entity = await _context.Manufactures.FirstOrDefaultAsync(m => m.Id == id);
            var dto = _mapper.Map<ManufactureDto>(entity);
            return dto;
        }

        public async Task<bool> UpdateAsync(UpdateManufactureDto dto)
        {
            var entity = await _context.Manufactures
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (entity == null)
            {
                return false;
            }
            entity = _mapper.Map(dto, entity);

            if(dto.Image != null)
            {
                var imageName = await _imageService.SaveImageAsync(dto.Image, Settings.ManufacturesPath);
                if (!string.IsNullOrEmpty(imageName))
                {
                    imageName = Path.Combine(Settings.ManufacturesPath, imageName);
                }

                if (!string.IsNullOrEmpty(entity.Image) && !string.IsNullOrEmpty(imageName))
                {
                    _imageService.DeleteImage(entity.Image);
                }

                entity.Image = imageName;
            }

            _context.Manufactures.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }
    }
}
