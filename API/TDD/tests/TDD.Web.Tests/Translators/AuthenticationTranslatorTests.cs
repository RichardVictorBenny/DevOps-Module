using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Services;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Shared.Options;
using TDD.Web.Translators;

namespace TDD.Web.Tests.Translators
{
    [TestFixture]   
    public class AuthenticationTranslatorTests
    {

        #region Register
        [Test]
        public void RegisterErrorWithoutViewMode()
        {

            var authSettings = new Mock<IAuthService>().Object;
            var jwtSettings = Options.Create(new JwtSettings {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var dataProtector = new Mock<IDataProtectionProvider>().Object;
            var userService = new Mock<IUserService>().Object;


            var translator = new AuthenticationTranslator(authSettings, jwtSettings, dataProtector, userService);


            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await translator.Register(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("viewModel"));
            });

        }

        #endregion

        #region xx
        
        public void Name()
        {
            //Arrange
            //Act
            //Asset
        }

        #endregion

        #region Login
        [Test]
        public void LoginErrorsWithoutViewModel()
        {

            var authSettings = new Mock<IAuthService>().Object;
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var dataProtector = new Mock<IDataProtectionProvider>().Object;
            var userService = new Mock<IUserService>().Object;


            var translator = new AuthenticationTranslator(authSettings, jwtSettings, dataProtector, userService);


            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { await translator.Login(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("viewModel"));
            });
        }

        #endregion

    }
}
