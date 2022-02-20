namespace EMTrackerDev.Models
{
    public class AnalysisResult
    {

        public int AnalysisResultId { get; set; }

        public int AnalysisId { get; set; }

        public string Component { get; set; }

        public string UOM { get; set; }

        public virtual Analysis Analysis { get; set; }
    }
}
