using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Test
    {
        public int TestID { get; set; }

        public string TestName { get; set; }

        public Sample Sample { get; set; }

        public User User { get; set; }
        public DateTime CollectionTime { get; set; }
        public string Instruments { get; set; }
        public TestType TestType { get; set; }
    
        public ICollection<Result> Result { get; set; }

    }
}
