using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public bool Active { get; set; }

    }
}
    