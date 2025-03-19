using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.DAL.Repositories.Cars
{
    public class CarRepository
        : GenericRepository<Car, string>,
        ICarRepository
    {
        public CarRepository(AppDbContext context)
        : base(context) { }

        public IQueryable<Car> GetCars(int page = 1, int pageSize = 10, Expression<Func<Car, bool>>? pred = null)
        {
            var entites = GetAll();

            if (pred != null)
            {
                entites = entites.Where(pred);
            }

            int count = entites.Count();

            pageSize = pageSize < 1 ? 10 : pageSize;
            page = page < 1 || page > count ? 1 : page;

            int pages = (int)Math.Ceiling((double)count / pageSize);

            var result = entites
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return result;
        }
    }
}
