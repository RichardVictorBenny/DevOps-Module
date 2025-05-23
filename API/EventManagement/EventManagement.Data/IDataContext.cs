using EventManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data
{
    public interface IDataContext
    {
        DbSet<Company> Companies { get; }
    }
}
