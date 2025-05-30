// File: DependencyInjection.cs
// Author: Richard Benny
// Purpose: Registers web layer services and translators for dependency injection in the application.

using System.Net.Security;
using TDD.Shared.Options;
using TDD.Shared.Providers;
using TDD.Web.Providers;
using TDD.Web.Translators;
using TDD.Web.Translators.Interfaces;

namespace TDD.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationTranslator, AuthenticationTranslator>();
            services.AddScoped<ITaskTranslator, TaskTranslator>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<JwtSettings>();

            return services;
        }
    }
}
