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
                            new Status{StatusName="Approved"},
                            new Status{StatusName="Rejected"}


                        };
                        stats.ForEach(s => context.Statuses.Add(s));
           // context.SaveChanges();

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
            // context.SaveChanges();


   


            var userRoles = new List<UserRole>
            {
                new UserRole{ Role="Employee"},
                new UserRole{Role="Manager"}
            };
            userRoles.ForEach(s => context.UserRoles.Add(s));
           //  context.SaveChanges();
            var users = new List<User>
            {
                new User{ UserRoleId=1,UserName="name",Password="password",FirstName="first",LastName="last"},
                new User{ UserRoleId=2,UserName="name",Password="password",FirstName="first2",LastName="last2"}

            };
            users.ForEach(s => context.Users.Add(s));
           //  context.SaveChanges();
            var locations = new List<LocationCode>
                        {
                            new LocationCode{LocationId="Mtn",Description="desc"}
                  

                        };
            locations.ForEach(s => context.Locationcodes.Add(s));
           // context.SaveChanges();
        }
    }
}
