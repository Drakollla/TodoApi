using System.ComponentModel.DataAnnotations;
using TodoApi.Data.Enums;

namespace TodoApi.DTOs.Todo
{
    public class CreateTodoItemDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public DateTime? DueDate { get; set; }
        public int? CategoryId { get; set; }
    }
}