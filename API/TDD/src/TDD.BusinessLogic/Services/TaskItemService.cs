using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Infrastructure.Data.Entities;
using TDD.Infrastructure.Data.Interfaces;
using TDD.Shared.Providers;

namespace TDD.BusinessLogic.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IDataContext dataContext;
        private readonly IUserProvider userProvider;

        public TaskItemService(IDataContext dataContext, IUserProvider userProvider)
        {
            this.dataContext = dataContext;
            this.userProvider = userProvider;
        }

        public async Task<Guid> Create(TaskModel task)
        {
            ArgumentNullException.ThrowIfNull(task, nameof(task));

            if (userProvider.UserId == Guid.Empty)
            {
                throw new InvalidOperationException("Cannot create task without Signing In");
            }

            var userId = userProvider.UserId;

            var entry = new TaskItem
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsFavorite = task.IsFavorite,
                IsHidden = task.IsHidden,
                UserId = userId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
            };

            await dataContext.Tasks.AddAsync(entry);
            task.Id = entry.Id;
            await dataContext.SaveChangesAsync();

            return task.Id;
        }

        public async Task Update(TaskModel task)
        {
            ArgumentNullException.ThrowIfNull(task);
            if (userProvider.UserId == Guid.Empty)
            {
                throw new InvalidOperationException("Cannot update task without Signing In");
            }

            var userId = userProvider.UserId;

            var existingEntry = await dataContext.Tasks.FirstOrDefaultAsync(x => x.Id == task.Id);
            if (existingEntry == null)
            {
                throw new InvalidOperationException("Task does not Exist");
            }

            if (existingEntry.UserId !=  userId)
            {
                throw new InvalidOperationException("Cannot update other users Tasks");
            }

            existingEntry.DueDate = task.DueDate;
            existingEntry.IsFavorite = task.IsFavorite;
            existingEntry.IsHidden = task.IsHidden;
            existingEntry.Title = task.Title;
            existingEntry.Description = task.Description;
            existingEntry.LastModifiedDate = DateTime.UtcNow;

            dataContext.Tasks.Update(existingEntry);
            await dataContext.SaveChangesAsync();

        }

        public async Task Delete(Guid Id)
        {
            if(userProvider.UserId == Guid.Empty)
            {
                throw new InvalidOperationException("Cannot delete task without Signing In");
            }

            var userId = userProvider.UserId;

            var entry = await dataContext.Tasks.FirstOrDefaultAsync(x => x.Id ==Id);
            if (entry == null)
            {
                throw new InvalidOperationException("Task does not exist");
            }
            if (entry.UserId != userId)
            {
                throw new InvalidOperationException("Cannot delete Task of a different user");
            }

            dataContext.Tasks.Remove(entry);
            await dataContext.SaveChangesAsync();

        }

        public Task<List<TaskModel>> GetAll()
        {
            if (userProvider.UserId == Guid.Empty)
            {
                throw new InvalidOperationException("Cannot retrieve tasks without Signing In");
            }
            var userId = userProvider.UserId;

            var tasks = dataContext.Tasks
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new TaskModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description ?? "",
                    DueDate = x.DueDate,
                    IsFavorite = x.IsFavorite,
                    IsHidden = x.IsHidden
                });

            return tasks.ToListAsync();
        }

        public Task<TaskModel> GetById(Guid Id)
        {
            if (userProvider.UserId == Guid.Empty)
            {
                throw new InvalidOperationException("Cannot retrieve task without Signing In");
            }
            var userId = userProvider.UserId;
        }
    }
}
