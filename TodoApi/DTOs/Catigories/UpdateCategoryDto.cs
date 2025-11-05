using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs.Catigories
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Название категории обязательно.")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}