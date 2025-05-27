using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Web.Translators;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Tests.Translators
{
    [TestFixture]
    public class TaskTranslatorTests
    {
        private Mock<ITaskItemService> _taskService;
        private ITaskTranslator _taskTranslator;
        [SetUp]
        public void SetUp() { 
            _taskService = new Mock<ITaskItemService>();
            _taskTranslator = new TaskTranslator(_taskService.Object);
            
        }

        #region Create

        [Test] 
public async Task CreateSuccessfulWithValidTask()
        {
            var task = new TaskViewModel
            {
                Title = "title",
                Description = "description",
                IsFavorite = true,
                IsHidden = false,
                DueDate = DateTime.Now,
            };

            var expectedResult = Guid.NewGuid();

            _taskService.Setup(x => x.Create(It.IsAny<TaskModel>())).ReturnsAsync(expectedResult);

            var result = await _taskTranslator.Create(task);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedResult));
                _taskService.Verify(x => x.Create(It.Is<TaskModel>(t =>
                    t.Title == task.Title &&
                    t.Description == task.Description &&
                    t.IsFavorite == task.IsFavorite &&
                    t.IsHidden == task.IsHidden &&
                    t.DueDate == task.DueDate
                )), Times.Once());
            });
        }

        #endregion


        #region GetbyId

        [Test]
        public async Task GetById_CallsServiceAndMapsResult()
        {
            var taskId = Guid.NewGuid();
            var taskModel = new TaskModel
            {
                Id = taskId,
                Title = "title"
            };
            var expectedResult = new TaskViewModel
            {
                Id = taskId,
                Title = "title"
            };

            _taskService.Setup(x => x.GetById(taskId)).ReturnsAsync(taskModel);

            var result = await _taskTranslator.GetById(taskId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(taskId));
                Assert.That(result.Title, Is.EqualTo(expectedResult.Title));
                _taskService.Verify(x => x.GetById(taskId), Times.Once);
            });
        }

        #endregion

        #region GetAll
        [Test]
        public async Task GetAll_CallsServiceAndReturnsMappedViewModels()
        {
            var tasks = new List<TaskModel>
                {
                    new TaskModel { Id = Guid.NewGuid(), Title = "Task 1", Description = "Desc 1" },
                    new TaskModel { Id = Guid.NewGuid(), Title = "Task 2", Description = "Desc 2" }
                };

            _taskService.Setup(s => s.GetAll()).ReturnsAsync(tasks);

            var result = await _taskTranslator.GetAll();

            Assert.Multiple(() => {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0].Title, Is.EqualTo("Task 1"));
                Assert.That(result[1].Description, Is.EqualTo("Desc 2"));
                _taskService.Verify(x => x.GetAll(), Times.Once);
            });
        }

        #endregion

        #region Update

        [Test]
        public async Task Update_CallsServiceWithMappedTaskModel()
        {
            var viewModel = new TaskViewModel
            {
                Id = Guid.NewGuid(),
                Title = "Test"
            };

            TaskModel capturedTaskModel = null;

            _taskService.Setup(x => x.Update(It.IsAny<TaskModel>()))
                .Callback<TaskModel>(x => capturedTaskModel = x)
                .Returns(Task.CompletedTask);

            await _taskTranslator.Update(viewModel);

            Assert.Multiple(() =>
            {
                Assert.That(capturedTaskModel, Is.Not.Null);
                Assert.That(capturedTaskModel.Id, Is.EqualTo(viewModel.Id));
                Assert.That(capturedTaskModel.Title, Is.EqualTo(viewModel.Title));
                Assert.That(capturedTaskModel.Description, Is.EqualTo(viewModel.Description));
                _taskService.Verify(s => s.Update(It.IsAny<TaskModel>()), Times.Once);
            });
        }

        #endregion

        #region Delete
        [Test]
        public async Task Delete_CallsServiceWithCorrectId()
        {
            var taskId = Guid.NewGuid();

            _taskService
                .Setup(s => s.Delete(taskId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _taskTranslator.Delete(taskId);

            _taskService.Verify(s => s.Delete(taskId), Times.Once);
        }
        #endregion


    }
}
