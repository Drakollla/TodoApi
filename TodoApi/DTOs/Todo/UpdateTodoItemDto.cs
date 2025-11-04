using System.ComponentModel.DataAnnotations;
using TodoApi.Data.Enums;

namespace TodoApi.DTOs.Todo
{
    public class UpdateTodoItemDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public PriorityLevel Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
    }
}