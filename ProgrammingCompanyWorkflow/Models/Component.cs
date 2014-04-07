using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgrammingCompanyWorkflow.Models
{
    public class Component
    {
        [Key]
        [Required(ErrorMessage = "Id не присвоился")]
        public Guid Id { get; set; }

        
        public string Name { get; set; }
    }
}