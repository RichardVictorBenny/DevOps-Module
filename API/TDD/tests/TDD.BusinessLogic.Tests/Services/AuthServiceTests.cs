using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Shared.Options;

namespace TDD.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserService> _userService;
        private IOptions<JwtSettings> _options;
        private AuthService _authService;

        [SetUp]
        public void SetUp() { 
            _userService = new Mock<IUserService>();
            _options = Options.Create(new JwtSettings
            {
                Key = "eyJhbGciOiJIUzUxMiJ9.eyJSb2xlIjoiVXNlciIsIklzc3VlciI6Iklzc3VlciIsIlVzZXJuYW1lIjoidGVzdEB0ZXN0LmNvbSIsImV4cCI6MTc0ODI2OTMxOSwiaWF0IjoxNzQ4MjY5MzE5fQ.DbQ9-mJPNKmL0mljiT23j_N2mHw8dGg0uAfJm_-IPKXY97xQe8RV4m-VNJUp_nKU80oKxyZKzMZl3BLAPFK--w\r\n",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            _authService = new AuthService(_userService.Object, _options);
        }

        #region Register
        [Test]  
        public void RegisterErrorWithoutModel()
        {

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await _authService.Register(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("model"));
            });

        }

        [Test]
        public async Task RegisterReturnTrueWhenUserCreatedSuccessfully()
        {
            var model = new RegisterModel
            {
                UserName = "test@test.com",
                Password = "Password@123",
                FirstName = "John",
                LastName = "Doe"
            };
            var failedResult = IdentityResult.Failed(new IdentityError { Description = "Simulated failure" });

            _userService.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(failedResult);

            var result = await _authService.Register(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RegisterReturnFalseWhenUserNotCreated()
        {
            var model = new RegisterModel
            {
                UserName = "test@test.com",
                Password = "Password@123",
                FirstName = "John",
                LastName = "Doe"
            };

            _userService.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.Register(model);

            Assert.That(result, Is.True);
        }

        #endregion

        #region Login
        [Test]
        public void LoginErrorWithModel() {


            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await _authService.Login(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("model"));
            });
        }

        [Test]
        public async Task LoginReturnsTrueWhenLoggedInSuccessfully()
        {
            var loginModel = new LoginModel
            {
                UserName = "test@test.com",
                Password = "Password@123"
            };
            var user = new ApplicationUser
            {
                UserName = loginModel.UserName
            };

            _userService.Setup(x => x.FindByEmailAsync(loginModel.UserName)).ReturnsAsync(user);
            _userService.Setup(x => x.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);

            var result = await _authService.Login(loginModel);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task LoginReturnsFalseWhenUserNotFound()
        {
            var loginModel = new LoginModel
            {
                UserName = "test@test.com",
                Password = "Password@123"
            };
            var user = new ApplicationUser
            {
                UserName = loginModel.UserName
            };

            _userService.Setup(x => x.FindByEmailAsync(loginModel.UserName)).ReturnsAsync((ApplicationUser)null);

            var result = await _authService.Login(loginModel);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task LoginReturnsFalseWhenPasswordDoesNotMatch()
        {
            var loginModel = new LoginModel
            {
                UserName = "test@test.com",
                Password = "Password@123"
            };
            var user = new ApplicationUser
            {
                UserName = loginModel.UserName,
            };

            _userService.Setup(x => x.FindByEmailAsync(loginModel.UserName)).ReturnsAsync(user);
            _userService.Setup(x => x.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(false);

            var result = await _authService.Login(loginModel);

            Assert.That(result, Is.False);
        }

        #endregion

        #region GenerateTokenString
        [Test]
        public void GenerateTokenStringErrorWithoutUser() {


            var ex = Assert.Throws<ArgumentNullException>( () => { _authService.GenerateTokenString(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("user"));
            });
        }

        [Test]
        public void GenerateTokenStringReturnsValidTokenString()
        {
            var user = new ApplicationUser { 
                UserName = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            } ;

            var token = this._authService.GenerateTokenString(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);



            Assert.Multiple(() =>
            {
                Assert.That(token, Is.Not.Null);
                Assert.That(jwtToken.Claims, Has.Some.Matches<Claim>(c => c.Type == ClaimTypes.Email && c.Value == user.UserName));
                Assert.That(jwtToken.Claims, Has.Some.Matches<Claim>(c => c.Type == ClaimTypes.Role && c.Value == "User"));
                Assert.That(jwtToken.Issuer, Is.EqualTo(_options.Value.Issuer));
                Assert.That(jwtToken.Audiences, Has.Member(_options.Value.Audience));
                Assert.That(jwtToken.ValidTo, Is.GreaterThan(DateTime.UtcNow));
            });

        }

        #endregion

        #region GenerateRefreshTokenString
        [Test]
        public void GenerateRefreshTokenStringErrorWithUser() {

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await _authService.GenerateRefreshTokenString(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("user"));
            });
        
        }

        [Test]
        public async Task GenerateRefreshTokenReturnsValidRefreshToken()
        {
            var user = new ApplicationUser
            {
                UserName = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _userService.Setup(u => u.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var token = await _authService.GenerateRefreshTokenString(user);

            Assert.That(token, Is.Not.Null);
        }
        #endregion
    }
}
