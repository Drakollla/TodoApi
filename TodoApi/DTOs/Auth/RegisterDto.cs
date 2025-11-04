using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs.Auth
{
    public record RegisterDto(

        [Required]
        [EmailAddress]
        string Email,

        [Required]
        string Password
    );
}