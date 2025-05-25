using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.Infrastructure.Data.Entities;

namespace TDD.BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterModel model);

        Task<bool> Login(LoginModel model);
        string GenerateTokenString(ApplicationUser user);
        Task<string> GenerateRefreshTokenString(ApplicationUser user);
    }
}
