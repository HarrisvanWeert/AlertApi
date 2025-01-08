using AlertApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlertApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Website> Websites { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<WebsiteAlert> WebsiteAlerts { get; set; }
    }
}
