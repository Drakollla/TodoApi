using TodoApi.Data.Models;
using TodoApi.DTOs.Todo;

namespace TodoApi.Mapping
{
    public static class TodoMappings
    {
        public static TodoItemDto ToDto(this TodoItem todoItem)
        {
            if (todoItem == null)
                return null;

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
                Priority = todoItem.Priority,
                CreatedDate = todoItem.CreatedDate,
                DueDate = todoItem.DueDate,
                CategoryId = todoItem.CategoryId,
                CategoryName = todoItem.Category?.Name
            };
        }

        public static TodoItem FromDto(this CreateTodoItemDto dto, int userId)
        {
            return new TodoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                UserId = userId,
            };
        }
    }
}