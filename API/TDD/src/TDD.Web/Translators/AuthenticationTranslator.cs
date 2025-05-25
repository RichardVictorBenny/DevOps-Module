using Microsoft.AspNetCore.Authentication;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators
{
    public class AuthenticationTranslator : IAuthenticationTranslator
    {
        private readonly IAuthService authService;

        public AuthenticationTranslator(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<bool> Register(LoginViewModel viewModel)
        {
            var model = Map(viewModel);
            return await this.authService.Register(model);
        }

        public async Task<bool> Login(LoginViewModel viewModel)
        {
            var model = Map(viewModel);
            return await this.authService.Login(model);
        }

        private LoginModel Map(LoginViewModel model)
        {
            return new LoginModel { 
                UserName = model.UserName,
                Password = model.Password,
            };

        }

    }
}
