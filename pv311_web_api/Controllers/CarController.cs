using Microsoft.AspNetCore.Mvc;
using pv311_web_api.DAL;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCars()
        {
            var cars = _context.Cars.ToList();
            return Ok(cars);
        }

        [HttpGet("carFirst")]
        public IActionResult GetCarFirst()
        {
            var car = _context.Cars.First();
            return Ok(car);
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] Car entity)
        {
            _context.Cars.Add(entity);
            _context.SaveChanges();
            return Ok($"Автомобіль {entity.Model} додано");
        }
    }
}
