using System;

namespace EMTrackerDev.Models
{
    public class TestResult
    {

        public int TestResultId { get; set; }

        public int AnalysisResultId { get; set; }
        public int? EnteredById { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public float? amount { get; set; }
        public virtual User EnteredBy { get; set; }

        public virtual AnalysisResult AnalysisResult{get; set;}
        public int TestId { get; set; }


        public virtual Test Test { get; set; }
    }
}
