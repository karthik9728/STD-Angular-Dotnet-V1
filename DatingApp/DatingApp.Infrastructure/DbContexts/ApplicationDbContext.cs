using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
