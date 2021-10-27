using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using EMTrackerDev.Models;

namespace EMTrackerDev.DAL
{
    public class SampleInitializer : System.Data.Entity. DropCreateDatabaseIfModelChanges<EMTContext>
        
    {
        protected override void Seed (EMTContext context)
        {
            var samples = new List<Sample>
            {
                new Sample{id=1,name="test",date=DateTime.Parse("2021-10-27")}
            };
            samples.ForEach(s => context.Samples.Add(s));
            context.SaveChanges();
            var results = new List<Result>
            {
                new Result{id=1,name="test"}
            };
            results.ForEach(s => context.Results.Add(s));
            context.SaveChanges();
        }
    }
}
