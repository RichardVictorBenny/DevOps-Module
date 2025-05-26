using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Shared.Models;

namespace TDD.Shared.Providers
{
    public interface IUserProvider
    {
        Guid UserId { get; }

        Task<UserModel> GetCurrentUser();
    }
}
