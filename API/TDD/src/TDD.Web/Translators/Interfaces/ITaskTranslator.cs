// File: ITaskTranslator.cs
// Author: Richard Benny
// Purpose: Defines the contract for translating task operations between view models and business logic.
using TDD.BusinessLogic.Models;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators.Interfaces
{
    public interface ITaskTranslator
    {
        Task<Guid> Create(TaskViewModel viewModel);
        Task     Delete(Guid Id);
        Task<List<TaskViewModel>> GetAll();
        Task<TaskViewModel> GetById(Guid Id);
        Task Update(TaskViewModel viewModel);
    }
}
