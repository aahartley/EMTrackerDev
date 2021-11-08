using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Result
    {
     
        public int ResultID { get; set; }

        public string ResultName { get; set; }
        public string Value { get; set; }

        public string UOM { get; set; }

        public DateTime ResultDateTime { get; set; }

        public Test Test { get; set; }

        public Sample Sample { get; set; }

  
    }
}
