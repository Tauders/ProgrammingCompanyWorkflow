using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgrammingCompanyWorkflow.Models
{
    public class ProjectMovement
    {
        public Guid Id { get; set; }
        public virtual Project Project { get; set; }
        public virtual DateTime ModifyTime { get; set; }
        public virtual ApplicationUser FromUser { get; set; }
        public virtual ApplicationUser ToUser { get; set; }
    }
}