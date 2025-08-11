using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Auth.ValidationLogic.Services;
using Compliance.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IConfiguration _configuration, IAuthenticationService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel request)
        {
            var user = await _authService.AuthenticateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            else
            {
                TokenLogic tokenLogic = new(_configuration);
                var accessToken = tokenLogic.GenerateAccessToken(user);
                var refreshToken = tokenLogic.GenerateRefreshToken(user.Username);

                var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(int.TryParse(_configuration["RefreshToken:ExpiryDays"], out var refreshExpiry) ? refreshExpiry : 7);

                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresAt = refreshTokenExpiresAt,
                    UserName = user.Username,
                    UserAgent = Request.Headers.UserAgent.ToString(),
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                await _authService.SaveRefreshTokenAsync(refreshTokenEntity);

                var response = new TokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpires = DateTime.UtcNow.AddMinutes(int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var expiry) ? expiry : 30),
                    RefreshTokenExpires = refreshTokenExpiresAt
                };

                // Setting the refresh token in a secure cookie
                SetRefreshTokenCookie(refreshToken, refreshTokenExpiresAt);

                return Ok(response);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel request)
        {
            bool result = await _authService.RegisterUserAsync(request);
            if (!result)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] TokenModel request)
        {
            if (string.IsNullOrEmpty(request.AccessToken))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                TokenLogic tokenLogic = new(_configuration);
                var principal = tokenLogic.ValidateJwtToken(request.AccessToken);
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

        private void SetRefreshTokenCookie(string refreshToken, DateTime Expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true if using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = Expires // Set cookie expiry
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}