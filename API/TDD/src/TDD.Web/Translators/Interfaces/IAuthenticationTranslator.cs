// File: IAuthenticationTranslator.cs
// Author: Richard Benny
// Purpose: Defines the contract for translating authentication operations between view models and business logic.
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators.Interfaces
{
    public interface IAuthenticationTranslator
    {
        Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(LoginViewModel viewModel);
        Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Refresh(RefreshTokenViewModel viewModel);
        Task<bool> Register(RegisterViewModel viewModel);
        Task<UserViewModel> GetCurrentUser();
    }
}
