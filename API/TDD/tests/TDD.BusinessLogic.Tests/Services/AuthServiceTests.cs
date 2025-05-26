using Microsoft.Extensions.Options;
using Moq;
using TDD.BusinessLogic.Services;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Shared.Options;

namespace TDD.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        #region Register
        [Test]  
        public async Task RegisterErrorWithoutModel()
        {
            var userService = new Mock<IUserService>();
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var service = new AuthService(userService.Object, jwtSettings);

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await service.Register(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("model"));
            });

        }

        #endregion

        #region Login
        [Test]
        public async Task LoginErrorWithModel() {
            var userService = new Mock<IUserService>();
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var service = new AuthService(userService.Object, jwtSettings);

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await service.Login(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("model"));
            });
        }

        #endregion

        #region GenerateTokenString
        [Test]
        public void GenerateTokenStringErrorWithoutUser() {
            var userService = new Mock<IUserService>();
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var service = new AuthService(userService.Object, jwtSettings);

            var ex = Assert.Throws<ArgumentNullException>( () => {  service.GenerateTokenString(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("user"));
            });
        }

        #endregion

        #region GenerateRefreshTokenString
        [Test]
        public async Task GenerateRefreshTokenStringErrorWithUser() {
            var userService = new Mock<IUserService>();
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var service = new AuthService(userService.Object, jwtSettings);

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await service.GenerateRefreshTokenString(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("user"));
            });
        }

        #endregion
    }
}
