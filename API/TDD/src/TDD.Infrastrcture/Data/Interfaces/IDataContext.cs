using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data.Entities;

namespace TDD.Infrastructure.Data.Interfaces
{
    public interface IDataContext

    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<TaskItem> Tasks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
