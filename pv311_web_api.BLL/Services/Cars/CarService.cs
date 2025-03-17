using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.DTOs.Cars;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Cars;
using pv311_web_api.DAL.Repositories.Manufactures;

namespace pv311_web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;

        public CarService(IMapper mapper, ICarRepository carRepository, IManufactureRepository manufactureRepository, IImageService imageService)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _manufactureRepository = manufactureRepository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if(!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository
                    .GetByNameAsync(dto.Manufacture);
            }

            if(dto.Images.Count() > 0)
            {
                string path = Path.Combine(Settings.CarsPath, entity.Id);
                _imageService.CreateDirectory(path);
                var carImages = await _imageService.SaveCarImagesAsync(dto.Images, path);
                entity.Images = carImages;
            
            }

            var result = await _carRepository.CreateAsync(entity);

            if(!result)
            {
                return new ServiceResponse("Не вдалося збрегети автомобіль");
            }

            return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' збережено", true);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _carRepository
                .GetAll()
                .Include(e => e.Images)
                .Include(e => e.Manufacture)
                .ToListAsync();

            var dtos = _mapper.Map<List<CarDto>>(entities);

            return new ServiceResponse("Автомобілі отримано", true, dtos);
        }
    }
}
