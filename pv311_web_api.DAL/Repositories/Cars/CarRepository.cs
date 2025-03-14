﻿using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL.Repositories.Cars
{
    public class CarRepository
        : GenericRepository<Car, string>,
        ICarRepository
    {
        public CarRepository(AppDbContext context)
        : base(context) { }
    }
}
