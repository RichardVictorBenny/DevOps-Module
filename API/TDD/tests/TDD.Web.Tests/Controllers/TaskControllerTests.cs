// File: TaskControllerTests.cs
// Author: Richard Benny
// Purpose: Contains unit tests for the TaskController, verifying controller construction and dependency validation.
using Moq;
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
    public class TaskControllerTests
    {
        #region ControllerTesting

        [Test]
        public void ControllerErrorWithoutTrasnlator()
        {

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new TaskController(null);
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
