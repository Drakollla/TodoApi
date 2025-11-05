using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs.User;
using TodoApi.Services.User;

namespace TodoApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetMyProfile()
        {
            var useerId = GetUserId();
            var userProfile = await _userService.GetMyProfileAsync(useerId);

            if (userProfile == null)
                return NotFound();

            return Ok(userProfile);
        }

        [HttpPost("me/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = GetUserId();
            var (success, errorMessage) = await _userService.ChangePasswordAsync(userId, dto);

            if (!success)
                return BadRequest(errorMessage);

            return Ok(new { message = "Пароль успешно изменён" });
        }

        [HttpDelete("{me}")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var userId = GetUserId();
            var success = await _userService.DeleteMyProfile(userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}