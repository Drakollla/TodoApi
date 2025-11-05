using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs.UserDtos;

namespace TodoApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return (false, "Пользователь не найден");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return (false, "Неверный текущий пароль");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();

            return (true, string.Empty);
        }

        public async Task<bool> DeleteMyProfile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto?> GetMyProfileAsync(int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Email = x.Email,
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}