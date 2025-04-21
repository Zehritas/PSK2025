using Microsoft.AspNetCore.Mvc;
using PSK2025.Models.DTO;
using PSK2025.ApiService.Services;

namespace PSK2025.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.RegisterAsync(request);
            if (!result) return BadRequest("User registration failed");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _userService.LoginAsync(request);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }

    }
}
