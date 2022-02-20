using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Test
    {
        public int TestID { get; set; }

        public int? SampleId { get; set; }

        public int? AnalysisResultId { get; set; }

        public virtual Sample Sample { get; set; }

        public virtual AnalysisResult AnalysisResult { get; set; }

      

    }
}
