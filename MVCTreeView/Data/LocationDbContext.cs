using Microsoft.EntityFrameworkCore;
using MVCTreeView.Models.MenuModels;

namespace MVCTreeView.Data
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> locations { get; set; }
    }
}
