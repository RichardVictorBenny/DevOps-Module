

using System.Net.Security;
using TDD.Web.Translators;
using TDD.Web.Translators.Interfaces;

namespace TDD.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationTranslator, AuthenticationTranslator>();

            return services;
        }
    }
}
