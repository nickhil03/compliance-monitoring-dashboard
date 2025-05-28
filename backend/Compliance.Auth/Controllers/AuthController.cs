using Compliance.Auth.ValidationLogic;
using Compliance.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IConfiguration _configuration, AuthenticationService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel request)
        {
            var user = _authService.AuthenticateAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            TokenLogic tokenLogic = new(_configuration);
            var token = tokenLogic.GenerateJwtToken(request.Username);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel request)
        {
            bool result = await _authService.RegisterUserAsync(request.Username, request.Password);
            if (!result)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            return Ok(new { message = "User registered successfully" });
        }


        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] TokenModel request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                TokenLogic tokenLogic = new(_configuration);
                var principal = tokenLogic.ValidateJwtToken(request.Token);
                if (principal != null)
                {
                    var claims = principal.Claims.Select(c => new { c.Type, c.Value });
                    return Ok(new { isValid = true, claims });
                }
                else
                {
                    return Unauthorized(new { isValid = false, message = "Invalid token" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { isValid = false, message = $"Token validation error: {ex.Message}" });
            }
        }
    }
}