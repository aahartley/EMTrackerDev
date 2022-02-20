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


            
                        var stats = new List<Status>
                        {
                            new Status{StatusName="Planned"},
                            new Status{StatusName="Collected"},
                            new Status{StatusName="In-Process"},
                            new Status{StatusName="Complete"},
                            new Status{StatusName="Approved"}

                        };
                        stats.ForEach(s => context.Statuses.Add(s));
         //   context.SaveChanges();

            var analyses = new List<Analysis>
                 {
                     new Analysis{Name="Metal"},
                     new Analysis{Name="Radioactivity"},
                     new Analysis{Name="Radionuclides"}
                 };
                 analyses.ForEach(a => context.Analyses.Add(a));

          //  context.SaveChanges();

            var analysisResults = new List<AnalysisResult>
                 {
                     new AnalysisResult{AnalysisId=1,Component="Aluminum",UOM="µg/L"},
                     new AnalysisResult{AnalysisId=1,Component="Lead",UOM="µg/L"}

                 };
                 analysisResults.ForEach(ar => context.AnalysisResults.Add(ar));
         //   context.SaveChanges();


            var samples = new List<Sample>
            {
                new Sample{StatusId=1,LocatedCodeId=1,CollectedDate=DateTime.Parse("2021-10-27"),
                amount=1,latitude="lat",longitude="long"}
            };
            samples.ForEach(s => context.Samples.Add(s));
          //  context.SaveChanges();

            var tests = new List<Test>
            {
                new Test{AnalysisResultId=1,SampleId=1}
            };
            tests.ForEach(t => context.Tests.Add(t));
         //  context.SaveChanges();

        }
    }
}
