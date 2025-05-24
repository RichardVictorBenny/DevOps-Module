using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Infrastrcture.Data;
using TDD.Infrastrcture.Data.Interfaces;

namespace TDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //interfaces  
            services.AddScoped<IDataContext, DataContext>();

            //setting up database  
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlDatabase")));

            return services;
        }
    }
}
