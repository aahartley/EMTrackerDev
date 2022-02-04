using System.Collections.Generic;

namespace EMTrackerDev.Models
{
    public class Analysis
    {
        public int AnalysisId { get; set; }
        public string Name { get; set; }
        public ICollection<Test> Test { get; set; }

    }
}
