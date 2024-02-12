using DatingApp.Domain.Models;
using DatingApp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
