using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Data;

namespace MetroWebApi.Models
{
    public class TrainContext : DbContext
    {
        public TrainContext(DbContextOptions<TrainContext> options)
           : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
    }
}
