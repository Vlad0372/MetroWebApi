using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Data;

namespace MetroWebApi.Models
{
    public class MetroContext : DbContext
    {
        public MetroContext(DbContextOptions<MetroContext> options)
           : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Train> Trains { get; set; }
    }
}
