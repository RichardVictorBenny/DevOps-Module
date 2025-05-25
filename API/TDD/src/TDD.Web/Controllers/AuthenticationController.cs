using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationTranslator translator;

        public AuthenticationController(IAuthenticationTranslator translator)
        {
            this.translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }
        [HttpPost("Login", Name = "LoginUser")]
        public async Task<bool> Login(LoginViewModel model)
        {
            return await translator.Login(model);
        }

        [HttpPost("Register", Name = "RegisterUser")]
        public async Task<bool> Register(LoginViewModel model)
        {
            return await translator.Register(model);
        }


    }
}
