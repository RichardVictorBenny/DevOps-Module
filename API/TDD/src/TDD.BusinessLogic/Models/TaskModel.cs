using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.BusinessLogic.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
