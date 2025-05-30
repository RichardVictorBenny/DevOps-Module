// File: AuthenticationControllerTests.cs
// Author: Richard Benny
// Purpose: Contains unit tests for the AuthenticationController, verifying controller construction and dependency validation.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Web.Controllers;
using TDD.Web.Translators.Interfaces;

namespace TDD.Web.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {

        #region ControllerTesting
        [Test]
        public void CannotCreateWithoutTranslator()
        {
            IAuthenticationTranslator translator = null;

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new AuthenticationController(translator);
            });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("translator"));
            });
        }
        #endregion
    }
}
