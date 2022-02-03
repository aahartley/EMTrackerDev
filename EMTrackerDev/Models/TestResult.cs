namespace EMTrackerDev.Models
{
    public class TestResult
    {

        public int TestResultId { get; set; }

        public int AnalysisReportId { get; set; }
        public int EnteredById { get; set; }

        public virtual User EnteredBy { get; set; }

        public virtual AnalysisResult AnalysisResults{get; set;}
        public int TestId { get; set; }


        public virtual Test Test { get; set; }
    }
}
