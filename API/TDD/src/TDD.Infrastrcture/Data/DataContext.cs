using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;

namespace TDD.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid >, IDataContext
    {
        public DataContext(DbContextOptions option): base(option) { }

    }
}
