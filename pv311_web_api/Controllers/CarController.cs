﻿using Microsoft.AspNetCore.Mvc;
using pv311_web_api.BLL.DTOs.Cars;
using pv311_web_api.BLL.Services.Cars;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : AppController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCarDto dto)
        {
            var response = await _carService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _carService.GetAllAsync();
            return CreateActionResult(response);
        }
    }
}
