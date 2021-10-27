using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }

        public virtual Sample Sample { get; set; }
    }
}
