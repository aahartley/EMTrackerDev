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
        public DbSet<EMTrackerDev.Models.Location> Locations { get; set; }
        public DbSet<EMTrackerDev.Models.User> Users { get; set; }
        public DbSet<EMTrackerDev.Models.UserRole> UserRoles { get; set; }
        public DbSet<EMTrackerDev.Models.Status> Status { get; set; }


        public DbSet<EMTrackerDev.Models.Test> Tests { get; set; }

        public DbSet<EMTrackerDev.Models.TestType> TestTypes { get; set; }
        public DbSet<EMTrackerDev.Models.SampleType> SampleTypes { get; set; }




    }
}
