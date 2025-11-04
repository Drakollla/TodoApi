using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs.Todo;
using TodoApi.Mapping;

namespace TodoApi.Services.Todo
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto dto, int userId)
        {
            var todoItem = dto.FromDto(userId);

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return todoItem.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
                return false;

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TodoItemDto>> GetAllAsync(int userId)
        {
            var todoItems = await _context.TodoItems
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return todoItems.Select(item => item.ToDto());
        }

        public async Task<TodoItemDto?> GetByIdAsync(int id, int userId)
        {
            var todoItem = await _context.TodoItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            return todoItem?.ToDto();
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, UpdateTodoItemDto dto, int userId)
        {
            var todoItem = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
                return (false, "Задача не найдена или у вас нет прав на ее изменение.");

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == dto.CategoryId.Value && c.UserId == userId);

                if (!categoryExists)
                    return (false, "Указанная категория не существует или не принадлежит вам.");
            }

            _context.Update(todoItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return (false, $"Произошла ошибка при обновлении базы данных: {ex.Message}");
            }

            return (true, null);
        }
    }
}