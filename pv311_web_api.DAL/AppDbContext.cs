using Microsoft.EntityFrameworkCore;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; }
    }
}
