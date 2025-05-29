using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDD.Web.Translators.Interfaces;
using TDD.Web.ViewModels;

namespace TDD.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskTranslator translator;

        public TaskController(ITaskTranslator translator)
        {
            this.translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await translator.GetAll();
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await translator.GetById(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskViewModel task)
        {
            var result = await translator.Create(task);
            return Ok(result);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await translator.Delete(id);
            return NoContent();
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(TaskViewModel task)
        {
            await translator.Update(task);
            return NoContent();
        }
    }
}
