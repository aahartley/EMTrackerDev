using EMTrackerDev.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EMTrackerDev.DAL
{
    public class EMTContext : DbContext
    {
        public EMTContext() : base("EMTContext")
        {

        }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<Result> Results { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
