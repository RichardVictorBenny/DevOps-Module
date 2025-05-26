using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data.Configurations;

namespace TDD.Infrastructure.Data.Entities
{
    [EntityTypeConfiguration(typeof(ApplicationUserConfiguration))]
    public class ApplicationUser : IdentityUser<Guid>
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }

    }
}
    