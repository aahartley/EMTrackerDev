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
                new Sample{SampleName="tst",Status={ },Location={ },Type={ },Amount=1,UOM="gram",Notes="testNotes",User={ },SampleDate=DateTime.Parse("2021-10-27")}
            };
            samples.ForEach(s => context.Samples.Add(s));
           //context.SaveChanges();

            var tests = new List<Test>
            {
                new Test{TestName="tst",Sample=samples[0],User={ },CollectionTime=DateTime.Parse("2021-10-27"),Instruments="ins",TestType={ } }
            };
            tests.ForEach(t => context.Tests.Add(t));
            //context.SaveChanges();

            var results = new List<Result>
           {
                new Result{ResultName="test",Value="1",UOM="gram",ResultDateTime=DateTime.Parse("2021-10-27"),Sample=samples[0],Test=tests[0]}
           };
            results.ForEach(s => context.Results.Add(s));
            //context.SaveChanges();
            Console.WriteLine("WE HERE");
        }
    }
}
