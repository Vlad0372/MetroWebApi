using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace MetroWebApi.Entities
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Railway> Railways { get; set; }
        public DbSet<TicketArchive> TicketArchives { get; set; }

    }
}
