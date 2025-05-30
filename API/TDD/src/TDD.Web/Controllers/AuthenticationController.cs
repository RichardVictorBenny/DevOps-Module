
// File: ApplicationUserConfiguration.cs
// Author: Richard Benny
// Purpose: Configures the ApplicationUser entity for Entity Framework, including property constraints and relationships.
using Microsoft.AspNetCore.Authorization;
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
                return BadRequest( error: "Something went wrong");
            }

            return Ok();
        }

        [HttpPost("Refresh", Name = "Refresh")]
        public async Task<IResult> Refresh(RefreshTokenViewModel refreshToken)
        {
            var result = await translator.Refresh(refreshToken);
            return result;
        }

        [Authorize, HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await translator.GetCurrentUser();
            if (user == null) return Unauthorized("User is not logged in");
            return Ok(user);
        }
    }
}
