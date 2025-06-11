using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("details")]
        public IActionResult GetUserDetails()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.Sid)?.Value,
                Name = User.FindFirst(ClaimTypes.Name)?.Value,
                Username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value)
            });
        }
    }
}