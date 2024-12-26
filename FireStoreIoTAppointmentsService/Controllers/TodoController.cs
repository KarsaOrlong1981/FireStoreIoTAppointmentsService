using FireStoreIoTAppointmentsService.Services;
using IoTAppointmentsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireStoreIoTAppointmentsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTodoLists()
        {
            var lists = await _todoService.GetAllTodoListsAsync();
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoListById(string id)
        {
            var list = await _todoService.GetTodoListByIdAsync(id);
            if (list == null) return NotFound();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateTodoList([FromBody] TodoList todoList)
        {
            if (todoList == null || string.IsNullOrEmpty(todoList.Name))
                return BadRequest("Invalid TodoList data.");

            await _todoService.AddOrUpdateTodoListAsync(todoList);
            return Ok("TodoList added/updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(string id)
        {
            await _todoService.DeleteTodoListAsync(id);
            return Ok("TodoList deleted successfully.");
        }

        [HttpGet("Task/ByListId/{listId}")]
        public async Task<IActionResult> GetTodoTasksByListId(string listId)
        {
            var tasks = await _todoService.GetTodoTasksByListIdAsync(listId);
            return Ok(tasks);
        }

        [HttpPost("Task")]
        public async Task<IActionResult> CreateOrUpdateTodoTask([FromBody] TodoTask todoTask)
        {
            if (todoTask == null || string.IsNullOrEmpty(todoTask.Description))
                return BadRequest("Invalid TodoTask data.");

            await _todoService.CreateOrUpdateTodoTaskAsync(todoTask);
            return Ok("TodoTask added/updated successfully.");
        }

        [HttpDelete("Task/{id}")]
        public async Task<IActionResult> DeleteTodoTask(string id)
        {
            await _todoService.DeleteTodoTaskAsync(id);
            return Ok("TodoTask deleted successfully.");
        }
    }

}
