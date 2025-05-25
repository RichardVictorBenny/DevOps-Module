using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Services;
using TDD.Shared.Options;
using TDD.Web.Translators;

namespace TDD.Web.Tests.Translators
{
    [TestFixture]
    public class AuthenticationTranslatorTests
    {

        #region Register

        public void RegisterErrorWithoutViewMode()
        {
            var authSettings = new Mock<AuthService>().Object;
            var jwtSettings = new Mock<IOptions<JwtSettings>>().Object;

            var translator = new AuthenticationTranslator(authSettings, jwtSettings);


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

        public void LoginErrorsWithoutViewModel()
        {
            var authSettings = new Mock<AuthService>().Object;
            var jwtSettings = new Mock<IOptions<JwtSettings>>().Object;

            var translator = new AuthenticationTranslator(authSettings, jwtSettings);


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
