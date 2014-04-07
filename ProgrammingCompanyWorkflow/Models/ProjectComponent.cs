using System;
using System.ComponentModel.DataAnnotations;

namespace ProgrammingCompanyWorkflow.Models
{
    public class ProjectComponent
    {
        [Key]
        [Required(ErrorMessage = "Id не присвоился")]
        public Guid Id { get; set; }

        public virtual Project Project { get; set; }

        public virtual Component Component { get; set; }

        [Required(ErrorMessage = "Необходимо количество деталей")]
        [Range(-1,Int32.MaxValue,ErrorMessage = "Число должно находиться в диапозоне от -1")]
        public int Count { get; set; }
    }
}