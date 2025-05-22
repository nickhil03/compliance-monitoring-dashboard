using Compliance.Auth.Model;
using Compliance.Auth.ValidationLogic;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration _configuration) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel request)
        {
            if (!Authentication.AuthenticateUser(request.Username, request.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            TokenLogic tokenLogic = new(_configuration);
            var token = tokenLogic.GenerateJwtToken(request.Username);
            return Ok(new { token });
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