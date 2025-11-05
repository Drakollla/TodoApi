using TodoApi.DTOs.Todo;

namespace TodoApi.Services.Todos
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDto>> GetAllAsync(int userId);
        Task<TodoItemDto?> GetByIdAsync(int id, int userId);
        Task<TodoItemDto> CreateAsync(CreateTodoItemDto dto, int userId);
        Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, UpdateTodoItemDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}