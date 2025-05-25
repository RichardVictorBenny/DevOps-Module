using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;

namespace TDD.BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(LoginModel model);

        Task<bool> Login(LoginModel model);
    }
}
