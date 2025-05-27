using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastructure.Data;
using TDD.Infrastructure.Data.Interfaces;

namespace TDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //interfaces  
            services.AddScoped<IDataContext, DataContext>();

            //setting up database  
            services.AddDbContext<DataContext>(options => options.UseSqlServer(
                Environment.GetEnvironmentVariable("ConnectionStrings__sqlDatabase") ??
                configuration.GetConnectionString("sqlDatabase") ??
                 throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            return services;
        }
    }
}
