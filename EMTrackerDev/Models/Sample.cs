using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Sample
    {
        public int id { get; set; }

        public string name { get; set; }
        public DateTime date { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
