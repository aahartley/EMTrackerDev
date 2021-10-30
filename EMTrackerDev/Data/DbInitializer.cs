using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace EMTrackerDev.Data
{
    public class DbInitializer 
        
    {
        public static void Initialize(EMTrackerDevContext context)
       {
            var samples = new List<Sample>
            {
                new Sample{Status="incomplete",Location={ },Type="fish",Amount=1,UOM="gram",Notes="testNotes",User={ },SampleDate=DateTime.Parse("2021-10-27")}
            };
            samples.ForEach(s => context.Samples.Add(s));
            context.SaveChanges();
 
            var results = new List<Result>
           {
                new Result{Component="test",Value=1,UOM="gram",Sample=samples[0]}
           };
            results.ForEach(s => context.Results.Add(s));
            context.SaveChanges();
            Console.WriteLine("WE HERE");
        }
    }
}
