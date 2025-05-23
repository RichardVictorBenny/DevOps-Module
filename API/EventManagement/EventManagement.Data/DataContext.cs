using EventManagement.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data
{
    public class DataContext : IdentityDbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }
        public DbSet<Company> Companies { get; set; }
    }
}
