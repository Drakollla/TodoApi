using TodoApi.Data.Enums;

namespace TodoApi.Data.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public PriorityLevel Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}