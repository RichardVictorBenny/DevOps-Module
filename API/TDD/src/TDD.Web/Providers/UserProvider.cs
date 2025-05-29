using Microsoft.AspNetCore.Mvc.Infrastructure;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Shared.Models;
using TDD.Shared.Providers;

namespace TDD.Web.Providers
{
    public class UserProvider : IUserProvider
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IUserService userService;
        private UserModel user = null;

        public UserProvider(
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor,
            IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.actionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        public Guid UserId
        {
            get
            {
                if (user == null)
                {
                    user = GetCurrentUser().Result;
                }
                return user.UserId;
            }
        }

        public async Task<UserModel> GetCurrentUser()
        {
            if (user == null)
            {
                var httpContext = httpContextAccessor.HttpContext ?? actionContextAccessor.ActionContext?.HttpContext;
                if (httpContext?.User == null)
                {
                    return null;
                }
                var applicationUser = await userService.GetUserAsync(httpContext.User);

                user = new UserModel
                {
                    UserId = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Email = applicationUser.Email,
                };
            }
            return user;
        }
    }
}
