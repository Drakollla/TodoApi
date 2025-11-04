using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs.Todo;
using TodoApi.Services.Todo;


namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAllTodos()
        {
            var userId = GetUserId();
            var todos = await _todoService.GetAllAsync(userId);

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoById(int id)
        {
            var userId = GetUserId();
            var todo = await _todoService.GetByIdAsync(id, userId);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateTodo(CreateTodoItemDto createDto)
        {
            var userId = GetUserId();
            var createdTodo = await _todoService.CreateAsync(createDto, userId);

            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, UpdateTodoItemDto updateDto)
        {
            var userId = GetUserId();
            var (success, errorMessage) = await _todoService.UpdateAsync(id, updateDto, userId);

            if (!success)
                return NotFound(new { message = errorMessage });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = GetUserId();
            var success = await _todoService.DeleteAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}