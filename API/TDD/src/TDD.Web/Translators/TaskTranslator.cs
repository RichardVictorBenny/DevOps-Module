using TDD.BusinessLogic.Models;
using TDD.BusinessLogic.Services.Interfaces;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Translators
{
    public class TaskTranslator : ITaskTranslator
    {
        private readonly ITaskItemService taskService;

        public TaskTranslator(ITaskItemService taskService)
        {
            this.taskService = taskService;
        }
        public async Task<Guid> Create(TaskViewModel viewModel)
        {
            return await this.taskService.Create(Map(viewModel));
        }

        public async Task Delete(Guid Id)
        {
            await this.taskService.Delete(Id);
        }

        public async Task<List<TaskViewModel>> GetAll()
        {
            var result =  await this.taskService.GetAll();
            return result.Select(Map).ToList();
        }

        public async Task<TaskViewModel> GetById(Guid Id)
        {
            return Map(await this.taskService.GetById(Id));
        }

        public async Task Update(TaskViewModel viewModel)
        {
            await this.taskService.Update(Map(viewModel));
        }

        internal TaskModel Map(TaskViewModel viewModel)
        {
            return new TaskModel
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Description = viewModel.Description,
                IsFavorite = viewModel.IsFavorite,
                IsHidden = viewModel.IsHidden,
                DueDate = viewModel.DueDate
            };
        }

        internal TaskViewModel Map(TaskModel model)
        {
            return new TaskViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IsFavorite = model.IsFavorite,
                IsHidden = model.IsHidden,
                DueDate = model.DueDate,
            };
        }
    }
}
