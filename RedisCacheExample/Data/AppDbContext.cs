using Microsoft.EntityFrameworkCore;
using RedisCacheExample.Models;

namespace RedisCacheExample.Data
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
