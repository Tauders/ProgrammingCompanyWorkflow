using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgrammingCompanyWorkflow.Models
{
    public class Project
    {
        [Key]
        [Required(ErrorMessage = "Id не присвоился")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Тема обязательна")]
        [Display(Name = "Тема ТЗ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тело ТЗ обязательно")]
        [Display(Name = "Тело ТЗ")]
        public string Body { get; set; }

        [Display(Name="Комментарии")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Дата и время создания ТЗ не присвоилось")]
        [Display(Name = "Дата и время создания ТЗ")]
        public DateTime CreatingTime { get; set; }

        [Display(Name = "Пользователь, создавший ТЗ")]
        public virtual ApplicationUser CreatingUser { get; set; }

        [Display(Name = "Время последнего изменения")]
        public DateTime LastModifyTime { get; set; }

        [Display(Name = "Текущий ответственный")]
        public virtual ApplicationUser CurrentResponseUser { get; set; }

        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }

        public virtual ICollection<ProjectComponent> ProjectComponents { get; set; }
        
    }
}