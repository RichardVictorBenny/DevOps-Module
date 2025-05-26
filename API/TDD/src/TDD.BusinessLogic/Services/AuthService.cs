using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Shared.Options;

namespace TDD.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings jwtSettings;
        private readonly IUserService userService;

        public AuthService(IUserService userService, IOptions<JwtSettings> settings)
        {
            this.jwtSettings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            this.userService = userService;
        }
        public async Task<bool> Register(RegisterModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var identityUser = new ApplicationUser
            { 
                UserName = model.UserName,
                Email = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await userService.CreateAsync(identityUser, model.Password);
            return result.Succeeded;
        }

        public async Task<bool> Login (LoginModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var identityUser = await this.userService.FindByEmailAsync(model.UserName);
            if(identityUser == null)
            {
                return false;
            }

           return  await this.userService.CheckPasswordAsync(identityUser, model.Password);


        }

        public async Task<string> GenerateRefreshTokenString(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user); ;
            string randomRefreshToken = null;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                randomRefreshToken = Convert.ToBase64String(randomNumber);
            }

            user.RefreshToken = randomRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(24); 
            await userService.UpdateAsync(user);


            return randomRefreshToken;
        }

        public string GenerateTokenString(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            IEnumerable<Claim> _claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                claims: _claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                signingCredentials: signingCred
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;

        }



    }
    
}
