using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.BusinessLogic.Models;

namespace TDD.BusinessLogic.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<Guid> Create(TaskModel task);
        Task Delete(Guid Id);
        Task<List<TaskModel>> GetAll();
        Task<TaskModel> GetById(Guid Id);
        Task Update(TaskModel task);
    }
}
