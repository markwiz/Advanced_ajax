using Microsoft.EntityFrameworkCore;
using AdvancedAjax.Models;

namespace AdvancedAjax.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<City> Cities { get; set; }
    }
} 