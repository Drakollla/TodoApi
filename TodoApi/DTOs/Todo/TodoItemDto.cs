using TodoApi.Data.Enums;

namespace TodoApi.DTOs.Todo
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public PriorityLevel Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}