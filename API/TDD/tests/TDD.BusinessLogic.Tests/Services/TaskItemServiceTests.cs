using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services;
using TDD.Infrastructure.Data;
using TDD.Infrastructure.Data.Entities;
using TDD.Shared.Providers;

namespace TDD.BusinessLogic.Tests.Services;

[TestFixture]
public class TaskItemServiceTests
{
    private DbContextOptions<DataContext> _dbOptions;
    private DataContext _context;
    private Mock<IUserProvider> _userProvider;
    private TaskItemService _taskService;

    [SetUp]
    public void SetUp()
    {
        _dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(_dbOptions);
        _userProvider = new Mock<IUserProvider>();
        _taskService = new TaskItemService(_context, _userProvider.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    #region Create

    [Test]
    public void Create_ErrorWithoutTask()
    {
        var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.Create(null));
        Assert.That(ex?.ParamName, Is.EqualTo("task"));
    }

    [Test]
    public void Create_ErrorWithoutSignedInUser()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Create(new TaskModel()));
        Assert.That(ex.Message, Is.EqualTo("Cannot create task without Signing In"));
    }

    [Test]
    public async Task Create_ReturnGuidWhenOperationSuccess()
    {
        var userId = Guid.NewGuid();
        _userProvider.Setup(x => x.UserId).Returns(userId);

        var result = await _taskService.Create(new TaskModel { Title = "Test Task" });

        var taskInDb = await _context.Tasks.FindAsync(result);
        Assert.That(taskInDb, Is.Not.Null);
        Assert.That(taskInDb.Title, Is.EqualTo("Test Task"));
        Assert.That(taskInDb.CreatedBy, Is.EqualTo(userId));
    }

    #endregion

    #region GetAll

    [Test]
    public void GetAll_ErrorWithoutSignedInUser()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.GetAll());
        Assert.That(ex.Message, Is.EqualTo("Cannot retrieve tasks without Signing In"));
    }

    [Test]
    public async Task GetAll_ReturnValidTaskModelArray()
    {
        var userId = Guid.NewGuid();
        _userProvider.Setup(x => x.UserId).Returns(userId);

        _context.Tasks.AddRange(
            new TaskItem { Id = Guid.NewGuid(), Title = "Test1", UserId = userId },
            new TaskItem { Id = Guid.NewGuid(), Title = "Test2", UserId = userId }
        );
        await _context.SaveChangesAsync();

        var result = await _taskService.GetAll();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Any(r => r.Title == "Test1"), Is.True);
        Assert.That(result.Any(r => r.Title == "Test2"), Is.True);
    }

    #endregion

    #region GetById

    [Test]
    public void GetById_ErrorWithoutValidUser()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.GetById(Guid.NewGuid()));
        Assert.That(ex.Message, Is.EqualTo("Cannot retrieve task without Signing In"));
    }

    [Test]
    public async Task GetById_ReturnValidTaskModel()
    {
        var userId = Guid.NewGuid();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "TaskX", CreatedBy = userId };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        _userProvider.Setup(x => x.UserId).Returns(userId);
        var result = await _taskService.GetById(task.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(task.Id));
        Assert.That(result.Title, Is.EqualTo("TaskX"));
    }

    [Test]
    public async Task GetById_ReturnsNullWhenTaskNotFound()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.NewGuid());
        var result = await _taskService.GetById(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    #endregion

    #region Update

    [Test]
    public void Update_ErrorWithoutSignedInUser()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Update(new TaskModel { Id = Guid.NewGuid(), Title = "X" }));
        Assert.That(ex.Message, Is.EqualTo("Cannot update task without Signing In"));
    }

    [Test]
    public async Task Update_ExistingTask_UpdatesPropertiesAndSaves()
    {
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var task = new TaskItem { Id = taskId, Title = "Old", CreatedBy = userId, UserId = userId };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        _userProvider.Setup(x => x.UserId).Returns(userId);
        await _taskService.Update(new TaskModel { Id = taskId, Title = "Updated" });

        var updated = await _context.Tasks.FindAsync(taskId);
        Assert.That(updated.Title, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task Update_ErrorWhenTaskNotFound()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.NewGuid());

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Update(new TaskModel { Id = Guid.NewGuid(), Title = "Updated" }));

        Assert.That(ex?.Message, Is.EqualTo("Task does not Exist"));
    }

    #endregion

    #region Delete

    [Test]
    public void Delete_ErrorWhenNotSignedIn()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.Empty);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Delete(Guid.NewGuid()));
        Assert.That(ex.Message, Is.EqualTo("Cannot delete task without Signing In"));
    }

    [Test]
    public async Task Delete_TaskDeletedSuccessfully()
    {
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, Title = "Delete Me", CreatedBy = userId, UserId = userId };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        _userProvider.Setup(x => x.UserId).Returns(userId);
        await _taskService.Delete(taskId);

        var deleted = await _context.Tasks.FindAsync(taskId);
        Assert.That(deleted, Is.Null);
    }
    [Test]
    public async Task Delete_ErrorWhenTaskNotFound()
    {
        _userProvider.Setup(x => x.UserId).Returns(Guid.NewGuid());

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Delete(Guid.NewGuid()));

        Assert.That(ex?.Message, Is.EqualTo("Task does not exist"));
    }

    [Test]
    public async Task Delete_ErrorWhenUserDoesNotOwnTheTaskToBeDeleted()
    {
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, Title = "Delete Me", CreatedBy = userId, UserId = userId };
        var secondaryUserId = Guid.NewGuid();

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        _userProvider.Setup(x => x.UserId).Returns(secondaryUserId);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Delete(taskId));

        Assert.That(ex?.Message, Is.EqualTo("Cannot delete Task of a different user"));
    }

    #endregion
}
