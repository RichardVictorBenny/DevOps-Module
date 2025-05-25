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
            var store = new Mock<IUserStore<ApplicationUser>>();

            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null, 
                null, 
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                null, 
                null, 
                null, 
                null 
            ).Object;

            var authSettings = new Mock<IAuthService>().Object;
            var jwtSettings = Options.Create(new JwtSettings {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var dataProtector = new Mock<IDataProtectionProvider>().Object;


            var translator = new AuthenticationTranslator(authSettings, jwtSettings,userManager, dataProtector);


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
            var store = new Mock<IUserStore<ApplicationUser>>();

            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null,
                null,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                null,
                null,
                null,
                null
            ).Object;

            var authSettings = new Mock<IAuthService>().Object;
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "test-key",
                Issuer = "test-issuer",
                Audience = "test-audience"
            });
            var dataProtector = new Mock<IDataProtectionProvider>().Object;


            var translator = new AuthenticationTranslator(authSettings, jwtSettings, userManager, dataProtector);


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
