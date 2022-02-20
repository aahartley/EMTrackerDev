using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Sample
    {

        public int SampleID { get; set; }

        public int StatusId { get; set; }
        public int? CollectedById { get; set; }
        public int? ApprovedById { get; set; }
        public int LocatedCodeId { get; set; }

        public DateTime CollectedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int? AnalysisId { get; set; }
        public double amount { get; set; }
        public string latitude { get; set; }
        public virtual LocationCode LocationCode { get; set; }
        public virtual User CollectedBy { get; set; }

        public virtual Analysis Analysis { get; set; }
        public virtual User ApprovedBy { get; set; }
        public string longitude { get; set; }
        public virtual Status Status { get; set; }
        public ICollection<Test>  Test { get; set; }




    }
}
