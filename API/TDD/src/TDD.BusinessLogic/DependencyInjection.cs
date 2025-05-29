using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Services;
using TDD.BusinessLogic.Services.Interfaces;

namespace TDD.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {

            services.AddScoped<IAuthService, AuthService>(); 
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskItemService, TaskItemService>();

            return services;
        }


    }
}
