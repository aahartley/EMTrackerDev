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

            //  Console.WriteLine("STATUS?!?!? "+statuses[0].StatusName);


            var stats = new List<Status>
            {
                new Status{StatusName="Planned"},
                new Status{StatusName="Collected"},
                new Status{StatusName="In-Process"},
                new Status{StatusName="Complete"},
                new Status{StatusName="Approved"}

            };
            stats.ForEach(s => context.Status.Add(s));
            // context.SaveChanges();
            var tests = new List<Test>
            {
                new Test{TestName="tst1",Sample={ },User={ },CollectionTime=DateTime.Parse("2021-10-27"),Instruments="ins",TestType={ } },
                new Test{TestName="tst2",Sample={ },User={ },CollectionTime=DateTime.Parse("2021-12-27"),Instruments="ins2",TestType={ } }

            };
            tests.ForEach(t => context.Tests.Add(t));
           // context.SaveChanges();





            var samples = new List<Sample>
            {
                new Sample{SampleName="tst",StatusId=1,Location={ },Type={ },Amount=1,UOM="gram",Notes="testNotes",User={ },SampleDate=DateTime.Parse("2021-10-27")}
            };
            samples.ForEach(s => context.Samples.Add(s));
         //  context.SaveChanges();



            var results = new List<Result>
           {
                new Result{ResultName="test",Value="1",UOM="gram",ResultDateTime=DateTime.Parse("2021-10-27"),Sample=samples[0],Test=tests[0]}
           };
            results.ForEach(s => context.Results.Add(s));
         //  context.SaveChanges();
            Console.WriteLine("WE HERE");
        }
    }
}
