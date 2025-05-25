using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;
using TDD.Shared.Options;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators
{
    public class AuthenticationTranslator : IAuthenticationTranslator
    {
        private readonly IAuthService authService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDataProtectionProvider dataProtection;
        private readonly JwtSettings jwtSettings;


        public AuthenticationTranslator(IAuthService authService, IOptions<JwtSettings> settings, UserManager<ApplicationUser> userManager, IDataProtectionProvider dataProtection)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.dataProtection = dataProtection ?? throw new ArgumentNullException(nameof(dataProtection));
            this.jwtSettings = settings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }
        public async Task<bool> Register(RegisterViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            var model = Map(viewModel);
            return await this.authService.Register(model);
        }

        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(LoginViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            var model = Map(viewModel);
            var result = await this.authService.Login(model);
            if (!result)
            {
                return TypedResults.Problem("Unauthorized", statusCode: StatusCodes.Status401Unauthorized);
            }

            var user = await this.userManager.FindByEmailAsync(model.UserName);

            var tokenString = this.authService.GenerateTokenString(user);
            var refreshToken = await this.authService.GenerateRefreshTokenString(user);
            var protector = dataProtection.CreateProtector("RefreshToken");
            var protectedRefreshToken = protector.Protect(refreshToken);

            var accessTokenResponse = new AccessTokenResponse
            {
                AccessToken = tokenString,
                ExpiresIn = 3600,  
                RefreshToken = protectedRefreshToken
            };
            return TypedResults.Ok(value: accessTokenResponse);
        }

        private LoginModel Map(LoginViewModel model)
        {
            return new LoginModel {
                UserName = model.UserName,
                Password = model.Password,
            };

        }

        private RegisterModel Map(RegisterViewModel model)
        {
            return new RegisterModel
            {
                UserName = model.UserName,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }

    }

}
