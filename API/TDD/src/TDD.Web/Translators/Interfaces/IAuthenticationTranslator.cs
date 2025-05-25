using TDD.Web.ViewModels;

namespace TDD.Web.Translators.Interfaces
{
    public interface IAuthenticationTranslator
    {
        Task<bool> Login(LoginViewModel viewModel);
        Task<bool> Register(LoginViewModel viewModel);
    }
}
