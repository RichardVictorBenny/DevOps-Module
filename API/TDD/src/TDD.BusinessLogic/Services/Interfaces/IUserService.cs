using Microsoft.AspNetCore.Identity;
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
    }
}