using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JwtAuthDemo.Data;
using JwtAuthDemo.Models;
using JwtAuthDemo.Models.DTOs;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /role (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _context.Roles
                .Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    Name = r.Name
                }).ToList();

            return Ok(roles);
        }

        // GET /role/{id} (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _context.Roles
                .Where(r => r.RoleId == id)
                .Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    Name = r.Name
                })
                .FirstOrDefault();

            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }

        // POST /role (Admin only) - Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateRole([FromBody] CreateRoleDto dto)
        {
            var role = new Role { Name = dto.Name };
            _context.Roles.Add(role);
            _context.SaveChanges();

            return Ok(new { message = "Role created successfully" });
        }

        // PUT /role/{id} (Admin only) - Update
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] UpdateRoleDto dto)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            role.Name = dto.Name;
            _context.SaveChanges();

            return Ok(new { message = "Role updated successfully" });
        }

        // DELETE /role/{id} (Admin only) - Delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return Ok(new { message = "Role deleted successfully" });
        }
    }
}
