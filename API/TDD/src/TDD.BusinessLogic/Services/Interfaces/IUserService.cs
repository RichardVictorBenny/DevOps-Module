using TDD.Infrastructure.Data.Entities;

namespace TDD.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> GetUserFromRefreshToken(string refreshToken);
    }
}