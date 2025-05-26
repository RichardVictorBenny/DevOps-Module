using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data.Configurations;

namespace TDD.Infrastructure.Data.Entities
{
    [EntityTypeConfiguration(typeof(TaskItemConfiguration))]
    public class TaskItem 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime LastModifiedBy { get; set; }
        public ApplicationUser User { get; set; }
            
    }
}
