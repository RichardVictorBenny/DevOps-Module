using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;

namespace TDD.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<bool> Register(LoginModel model)
        {
            var identityUser = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.UserName
            };

            var result = await userManager.CreateAsync(identityUser, model.Password);
            return result.Succeeded;
        }

        public async Task<bool> Login (LoginModel model)
        {
            var identityUser = await this.userManager.FindByEmailAsync(model.UserName);
            if(identityUser == null)
            {
                return false;
            }

            return await this.userManager.CheckPasswordAsync(identityUser, model.Password);

        }
    }
}
