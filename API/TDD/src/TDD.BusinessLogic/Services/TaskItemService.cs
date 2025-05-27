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
            throw new NotImplementedException();
        }

        public async Task Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TaskModel> GetById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
