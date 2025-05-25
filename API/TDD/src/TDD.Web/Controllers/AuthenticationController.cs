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
        public async Task<IResult> Login(LoginViewModel model)
        {
            var result =  await translator.Login(model);
            return result;
        }

        [HttpPost("Register", Name = "RegisterUser")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await translator.Register(model);
            if (!result)
            {
                return BadRequest("Something went wrong");
            }

            return Ok("Registration successful");
        }
    }
}
