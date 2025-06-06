﻿// File: AuthenticationTranslator.cs
// Author: Richard Benny
// Purpose: Implements translation between authentication view models and business logic, handling registration, login, token refresh, and user retrieval.
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;
using TDD.Shared.Models;
using TDD.Shared.Options;
using TDD.Shared.Providers;
using TDD.Web.Providers;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators
{
    public class AuthenticationTranslator : IAuthenticationTranslator
    {
        private readonly IAuthService authService;
        private readonly IDataProtectionProvider dataProtection;
        private readonly IUserService userService;
        private readonly IUserProvider userProvider;
        private readonly JwtSettings jwtSettings;


        public AuthenticationTranslator(
            IAuthService authService, 
            IOptions<JwtSettings> settings, 
            IDataProtectionProvider dataProtection, 
            IUserService userService, 
            IUserProvider userProvider
         )
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.dataProtection = dataProtection ?? throw new ArgumentNullException(nameof(dataProtection));
            this.userService = userService;
            this.userProvider = userProvider;
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

            var user = await this.userService.FindByEmailAsync(model.UserName);

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

            public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Refresh(RefreshTokenViewModel viewModel)
            {
                ArgumentNullException.ThrowIfNull(viewModel);
                var protector = dataProtection.CreateProtector("RefreshToken");
                string rawRefreshToken;
                try
                {
                    rawRefreshToken = protector.Unprotect(viewModel.RefreshToken);
                }
                catch (CryptographicException)
                {
                    return TypedResults.Problem("Invalid refresh token", statusCode: StatusCodes.Status400BadRequest);
                }

                var user = await this.userService.GetUserFromRefreshToken(rawRefreshToken);


                if (user == null || user.RefreshTokenExpiry!.Value < DateTime.UtcNow)
                {
                    return TypedResults.Problem("Expired refresh token", statusCode: StatusCodes.Status400BadRequest);
                }

                //check for active user

                var tokenString = this.authService.GenerateTokenString(user);
                var newRefreshToken = await this.authService.GenerateRefreshTokenString(user);
                var protectedRefreshToken = protector.Protect(viewModel.RefreshToken);

                var accessTokenResponse = new AccessTokenResponse
                {
                    AccessToken = tokenString,
                    ExpiresIn = 3600,
                    RefreshToken = protectedRefreshToken
                };
                return TypedResults.Ok(value: accessTokenResponse);
            }

        public async Task<UserViewModel?> GetCurrentUser()
        {
            var user = await userProvider.GetCurrentUser();
            if (user == null) return null;
            return Map(user);
        }

        private LoginModel Map(LoginViewModel model)
        {
            return new LoginModel {
                UserName = model.Username,
                Password = model.Password,
            };

        }

        private RegisterModel Map(RegisterViewModel model)
        {
            return new RegisterModel
            {
                UserName = model.Username,
                Password = model.Password,
                FirstName = model.Firstname,
                LastName = model.Lastname
            };
        }

        private UserViewModel Map(UserModel model)
        {
            return new UserViewModel
            {
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
        }
    }

}
