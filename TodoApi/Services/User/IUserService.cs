using TodoApi.DTOs.User;

namespace TodoApi.Services.User
{
    public interface IUserService
    {
        Task<UserDto?> GetMyProfileAsync(int userId);
        Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> DeleteMyProfile(int userId);
    }
}