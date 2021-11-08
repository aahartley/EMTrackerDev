using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Sample
    {
    
        public int SampleID { get; set; }

        public string SampleName { get; set;  }

        public Status Status { get; set; }

        public Location Location { get; set; }

        public SampleType Type { get; set; }

        public double Amount { get; set; }

        public string UOM { get; set; }

        public string Notes { get; set; }

        public User User { get; set; }

        public DateTime SampleDate { get; set; }

        public ICollection<Result> Result { get; set; }
        public ICollection<Test>  Test { get; set; }




    }
}
