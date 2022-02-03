using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Models
{
    public class User
    {

        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
