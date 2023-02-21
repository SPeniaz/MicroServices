using Microsoft.EntityFrameworkCore;
using PaltformService.Models;

namespace PaltformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; } = null!;
    }
}