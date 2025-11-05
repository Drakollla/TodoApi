using TodoApi.DTOs.UserDtos;

namespace TodoApi.Services.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetMyProfileAsync(int userId);
        Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<bool> DeleteMyProfile(int userId);
    }
}