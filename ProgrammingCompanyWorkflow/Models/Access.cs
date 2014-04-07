using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProgrammingCompanyWorkflow.Models
{
    public class Access
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public virtual ApplicationRole FromRole { get; set; }
        public virtual ApplicationRole ToRole { get; set; }
    }
}