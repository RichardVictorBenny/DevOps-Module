using System.ComponentModel.DataAnnotations;

namespace TDD.Web.ViewModels
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
