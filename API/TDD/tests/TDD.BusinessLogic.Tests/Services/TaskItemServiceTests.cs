using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;
using TDD.Shared.Providers;

namespace TDD.BusinessLogic.Tests.Services
{
    [TestFixture]
    public class TaskItemServiceTests
    {
        private Mock<IDataContext> _context;
        private Mock<IUserProvider> _userProvider;
        private TaskItemService _taskService;
        private Mock<DbSet<TaskItem>> _mockTaskSet;

        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IDataContext>();
            _userProvider = new Mock<IUserProvider>();
            _taskService = new TaskItemService(_context.Object, _userProvider.Object);
            _mockTaskSet = new Mock<DbSet<TaskItem>>();
        }

        #region Create
        [Test]
        public void Create_ErrorWithoutTask()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => { _ = await _taskService.Create(null); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex?.ParamName, Is.EqualTo("task"));
            });
        }

        [Test]
        public void Create_ErrorWithoutSignedInUser()
        {
            var task = new TaskModel();

            _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => { await _taskService.Create(task); });

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Cannot create task without Signing In"));
            });
        }

        [Test]
        public async Task Create_ReturnGuidWhenOperationSuccess()
        {
            var task = new TaskModel { Title = "Test" };
            var userId = Guid.NewGuid();
            var expectedTaskId = Guid.NewGuid();

            _userProvider.Setup(x => x.UserId).Returns(userId);

            _mockTaskSet.Setup(x => x.AddAsync(It.IsAny<TaskItem>(), default))
                .ReturnsAsync((TaskItem t, CancellationToken _) => {
                    t.Id = expectedTaskId;
                    return Mock.Of<EntityEntry<TaskItem>>();
                });

            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);

            var result = await _taskService.Create(task);

            Assert.That(result, Is.EqualTo(expectedTaskId));
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockTaskSet.Verify(x => x.AddAsync(It.IsAny<TaskItem>(), default), Times.Once);

        }
        #endregion

        #region GetAll
        [Test]
        public void GetAll_ErrorWithoutSignedInUser()
        {
            _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _taskService.GetAll());

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Cannot retrieve tasks without Signing In"));
            });
        }

        [Test]
        public async Task GetAll_ReturnValidTaskModelArray()
        {
            var expectedTasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "Test1" },
                new TaskItem { Id = Guid.NewGuid(), Title = "Test2" }
            };
            var userId = Guid.NewGuid();

            expectedTasks.ForEach(t => t.CreatedBy = userId);
            _userProvider.Setup(x => x.UserId).Returns(userId);

            var queryableTasks = expectedTasks.AsQueryable();

            SetupMockTaskSetWithQueryable(queryableTasks);


            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);

            var result = await _taskService.GetAll();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(expectedTasks.Count));

                foreach (var expected in expectedTasks)
                {
                    var actual = result.SingleOrDefault(r => r.Id == expected.Id);
                    Assert.That(actual, Is.Not.Null);
                    Assert.That(actual.Title, Is.EqualTo(expected.Title));
                }
            });
            
        }
        #endregion

        #region GetById
        [Test]
        public void GetById_ErrorWithoutValidUser()
        {
            _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _taskService.GetById(Guid.NewGuid()));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Cannot retrieve task without Signing In"));
            });
        }

        [Test]
        public async Task GetById_ReturnValidTaskModel()
        {
            var userId = Guid.NewGuid();
            var expectedTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                CreatedBy = userId,
            };

            _userProvider.Setup(x => x.UserId).Returns(userId);

            var taskList = new List<TaskItem> { expectedTask }.AsQueryable();

            SetupMockTaskSetWithQueryable(taskList);


            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);

            var result = await _taskService.GetById(expectedTask.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(expectedTask.Id));
                Assert.That(result.Title, Is.EqualTo(expectedTask.Title));
            });

            _context.Verify(x => x.Tasks, Times.Once);
        }


        [Test]
        public async Task GetById_ReturnsNullWhenTaskNotFound()
        {
            var userId = Guid.NewGuid();
            _userProvider.Setup(x => x.UserId).Returns(userId);

            // Simulate an empty DbSet (no task found)
            var taskList = new List<TaskItem>().AsQueryable();

            SetupMockTaskSetWithQueryable(taskList);


            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);

            var result = await _taskService.GetById(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        #endregion

        #region Update
        [Test]
        public void Update_ErrorWithoutSignedInUser()
        {
            var updatedTask = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Updated",
            };
            _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _taskService.Update(updatedTask));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Cannot update task without Signing In"));
            });
        }

        [Test]
        public async Task Update_ExistingTask_UpdatesPropertiesAndSaves()
        {
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Old Title",
                CreatedBy = userId
            };

            var updatedTaskModel = new TaskModel
            {
                Id = taskId,
                Title = "New Title"
            };

            _userProvider.Setup(x => x.UserId).Returns(userId);

            var taskList = new List<TaskItem> { existingTask }.AsQueryable();

            SetupMockTaskSetWithQueryable(taskList);


            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _taskService.Update(updatedTaskModel);

            Assert.Multiple(() =>
            {
                Assert.That(existingTask.Title, Is.EqualTo(updatedTaskModel.Title));
                _mockTaskSet.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Once);
                _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            });
        }


        [Test]
        public async Task Update_DoesNothingWhenTaskNotFound()
        {
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _userProvider.Setup(x => x.UserId).Returns(userId);

            var taskList = new List<TaskItem>
                {
                    new TaskItem
                    {
                        Id = Guid.NewGuid(),
                        Title = "Unrelated Task",
                        CreatedBy = userId
                    }
                }.AsQueryable();

            SetupMockTaskSetWithQueryable(taskList);

            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);

            await _taskService.Update(new TaskModel { Id = taskId, Title = "Shouldn't matter" });

            _mockTaskSet.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Never);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }



        #endregion

        #region Delete

        [Test]
        public async Task Delete_ErrorWhenNotSignedIn()
        {
            var taskId = Guid.NewGuid();
            _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _taskService.Delete(taskId));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Cannot Delete task without Signing In"));
            });
        }

        [Test]
        public async Task Delete_TaskDeletedSuccessfully()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var taskItem = new TaskItem
            {
                Id = taskId,
                Title = "Test",
                CreatedBy = userId,
            };

            _userProvider.Setup(x => x.UserId).Returns(userId);

            var taskList = new List<TaskItem> { taskItem }.AsQueryable();

            SetupMockTaskSetWithQueryable(taskList);


            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _taskService.Delete(taskId);

            Assert.Multiple(() =>
            {
                _mockTaskSet.Verify(x => x.Remove(It.Is<TaskItem>(t => t.Id == taskId)), Times.Once);
                _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            });
        }

        #endregion

        private void SetupMockTaskSetWithQueryable(IQueryable<TaskItem> taskList)
        {
            _mockTaskSet.As<IQueryable<TaskItem>>().Setup(m => m.Provider).Returns(taskList.Provider);
            _mockTaskSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(taskList.Expression);
            _mockTaskSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(taskList.ElementType);
            _mockTaskSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(() => taskList.GetEnumerator());

            _context.Setup(x => x.Tasks).Returns(_mockTaskSet.Object);
        }

    }
}
