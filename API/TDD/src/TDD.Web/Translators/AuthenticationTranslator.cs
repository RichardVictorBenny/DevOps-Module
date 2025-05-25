using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Shared.Options;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators
{
    public class AuthenticationTranslator : IAuthenticationTranslator
    {
        private readonly IAuthService authService;
        private readonly JwtSettings jwtSettings;

        public AuthenticationTranslator(IAuthService authService, IOptions<JwtSettings> settings)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.jwtSettings = settings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }
        public async Task<bool> Register(LoginViewModel viewModel)
        {
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

            var tokenString = this.authService.GenerateTokenString(model);
            var accessTokenResponse = new AccessTokenResponse
            {
                AccessToken = tokenString,
                ExpiresIn = 3600,  
                RefreshToken = "sample-refresh-token"
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

    }

}
