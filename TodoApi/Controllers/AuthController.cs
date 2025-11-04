using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.Auth;
using TodoApi.Services.Auth;

namespace TodoApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var (success, errorMessage) = await _authService.RegisterAsync(registerDto);

            if (!success)
                return BadRequest(new { message = errorMessage });

            return Ok(new { message = "Регистрация прошла успешно." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var (success, data, errorMessage) = await _authService.LoginAsync(loginDto);

            if (!success)
                return Unauthorized(new { message = "Неверный email или пароль." });
            
            return Ok(data);
        }
    }
}