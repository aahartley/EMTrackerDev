using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class Status
    {   
        public Status()
        {
            StatusId = 1;
        }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
