// File: IAuthService.cs
// Author: Richard Benny
// Purpose: Defines the contract for authentication services, including user registration, login, and token generation.

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TDD.Infrastructure.Data.Entities;

namespace TDD.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> GetUserFromRefreshToken(string refreshToken);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user);
    }
}