﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Manufactures;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Manufactures;

namespace pv311_web_api.BLL.Services.Manufactures
{
    public class ManufactureService : IManufactureService
    {
        private readonly IMapper _mapper;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;

        public ManufactureService(IMapper mapper, IImageService imageService, IManufactureRepository manufactureRepository)
        {
            _mapper = mapper;
            _imageService = imageService;
            _manufactureRepository = manufactureRepository;
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

            var result = await _manufactureRepository.CreateAsync(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);
            if(entity == null)
            {
                return false;
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            var result = await _manufactureRepository.DeleteAsync(entity);
            return result;
        }

        public async Task<ManufactureDto?> GetByIdAsync(string id)
        {
            var entity = await _manufactureRepository.GetByIdAsync(id);
            var dto = _mapper.Map<ManufactureDto>(entity);
            return dto;
        }

        public async Task<bool> UpdateAsync(UpdateManufactureDto dto)
        {
            var entity = await _manufactureRepository
                .GetAll()
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

            var result = await _manufactureRepository.UpdateAsync(entity);
            return result;
        }
    }
}
