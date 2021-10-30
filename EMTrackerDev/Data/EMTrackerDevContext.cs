using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Models;

namespace EMTrackerDev.Data
{
    public class EMTrackerDevContext : DbContext
    {
        public EMTrackerDevContext (DbContextOptions<EMTrackerDevContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EMTrackerDev.Models.Sample> Samples { get; set; }
        public DbSet<EMTrackerDev.Models.Result> Results { get; set; }
        public DbSet<EMTrackerDev.Models.Location> Location { get; set; }
        public DbSet<EMTrackerDev.Models.User> User { get; set; }


    }
}
