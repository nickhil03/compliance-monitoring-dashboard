using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Domain.Model;
using Compliance.Domain.Repositories.TokenModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IAuthenticationService _authService, ITokenLogic _tokenLogic) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel request)
        {
            try
            {
                if (await _authService.AuthenticateUserAsync(request.Username, request.Password))
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var userAgent = Request.Headers.UserAgent.ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var response = await _tokenLogic.GenerateTokens(request.Username, userAgent, ipAddress, false);

                // Setting the refresh token in a secure cookie
                SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpires);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel request)
        {
            try
            {
                bool result = await _authService.RegisterUserAsync(request);
                if (!result)
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred during registration" });
            }            
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(Request.Cookies["refreshToken"]))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                var principal = _tokenLogic.ValidateJwtToken(request.Token);
                if (principal == null)
                {
                    return Unauthorized(new { isValid = false, message = "Invalid token" });
                }

                var claims = principal.Claims.Select(c => new { c.Type, c.Value });
                return Ok(new { isValid = true, claims });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isValid = false, message = $"Token validation error: {ex.Message}" });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token are required" });
            }
            try
            {
                var userName = await _tokenLogic.GetUserNameFromRefreshTokenAsync(refreshToken);
                if (userName == null)
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                var userAgent = Request.Headers.UserAgent.ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var response = await _tokenLogic.GenerateTokens(userName, userAgent, ipAddress, true);
                if (response == null)
                {
                    return Unauthorized(new { message = "Could not generate new tokens" });
                }

                await _tokenLogic.RevokeRefreshTokenAsync(refreshToken, response.RefreshToken);

                // Update the refresh token cookie
                SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpires);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error refreshing token: {ex.Message}" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _tokenLogic.RevokeRefreshTokenAsync(refreshToken);
                }
                
                // Remove the refresh token cookie
                Response.Cookies.Delete("refreshToken");

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error during logout" });
            }
        }

        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAllTokens()
        {
            try
            {
                var userId = User.FindFirst("_id")?.Value ?? "0";
                await _tokenLogic.RevokeAllUserTokensAsync(userId);

                return Ok(new { message = "All tokens revoked successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error revoking tokens" });
            }
        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime Expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true if using HTTPS
                SameSite = SameSiteMode.None,
                Expires = Expires, // Set cookie expiry
                IsEssential = true
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}