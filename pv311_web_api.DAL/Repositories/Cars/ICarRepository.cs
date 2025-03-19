using pv311_web_api.DAL.Entities;
using System.Linq.Expressions;

namespace pv311_web_api.DAL.Repositories.Cars
{
    public interface ICarRepository
        : IGenericRepository<Car, string>
    {
        IQueryable<Car> GetCars(int page = 1, int pageSize = 10, Expression<Func<Car, bool>>? pred = null);
    }
}
