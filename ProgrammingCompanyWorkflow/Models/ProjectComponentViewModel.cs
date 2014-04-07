using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgrammingCompanyWorkflow.Models
{
    public class ProjectComponentViewModel
    {
        public string ComponentId { get; set; }
        public string CountView { get; set; }

    }
    
    public class TemporalClass
    {
        public Project Project { get; set; }
        public List<ProjectComponentViewModel> ProjectComponents { get; set; }
    }
}