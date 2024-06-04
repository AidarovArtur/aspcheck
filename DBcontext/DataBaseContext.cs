using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using task1.Models;

namespace task1.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql(Configuration.GetConnectionString("WebApiDataBase"));
            }
        }

        public DbSet<User> Users { get; set; }
    }
}

