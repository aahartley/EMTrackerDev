using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class LocationCode

    {
        public int LocationCodeId { get; set; }
        public string LocationId { get; set; }

        public string Description { get; set; }


    
    }
}
