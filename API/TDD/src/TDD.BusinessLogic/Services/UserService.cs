using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;

namespace TDD.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext dataContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IDataContext dataContext, UserManager<ApplicationUser> userManager)
        {
            this.dataContext = dataContext;
            this.userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserFromRefreshToken(string refreshToken)
        {
            var users = this.userManager.Users;
            return await users.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            return user;
        }
    }
}
