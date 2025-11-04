using TodoApi.DTOs.Auth;

namespace TodoApi.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, AuthResponseDto? Data, string ErrorMessage)> LoginAsync(LoginDto dto);
    }
}