using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleTestController : ControllerBase
    {
        // Accessible by any authenticated user
        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { username, role });
        }


        // Accessible only by Admin role
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new { message = "This endpoint is restricted to Admins." });
        }

        // Accessible only by User role
        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public IActionResult UserOnly()
        {
            return Ok(new { message = "This endpoint is restricted to Users." });
        }

        // Accessible by either Admin or User role
        [Authorize(Roles = "Admin,User")]
        [HttpGet("common")]
        public IActionResult Common()
        {
            return Ok(new { message = "Both Admins and Users can access this endpoint." });
        }
    }
}
