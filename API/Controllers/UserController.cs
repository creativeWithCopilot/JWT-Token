using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JwtAuthDemo.Data;
using JwtAuthDemo.Models;
using JwtAuthDemo.Models.DTOs;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public UserController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST /user/login - validates credentials, returns JWT + user info.
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginRequest)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var hasher = new PasswordHasher<User>(); // Use ASP.NET Core's built-in password hasher to verify the password
            var result = hasher.VerifyHashedPassword(user, user.Password, loginRequest.Password); // Compare the provided password with the stored hashed password

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Invalid username or password" });

            var token = _tokenService.GenerateToken(user.Username, user.Role.Name);
            return Ok(new 
            { 
                token,
                user = new { user.UserId, user.Username, Role = user.Role.Name }
            });
        }

        // GET /user (Admin only) - Read all users
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    RoleName = u.Role.Name
                })
                .ToList();

            return Ok(users);
        }

        // POST /user (Admin only) - Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            var hasher = new PasswordHasher<User>(); // Use ASP.NET Core's built-in password hasher
            var user = new User
            {
                Username = dto.Username,
                Password = hasher.HashPassword(new User(), dto.Password), // Hash the password before storing
                RoleId = dto.RoleId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "User created successfully" });
        }

        // PUT /user/{id} (Admin only) - Update
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            user.Username = dto.Username;
            user.Password = dto.Password;
            user.RoleId = dto.RoleId;

            _context.SaveChanges();

            return Ok(new { message = "User updated successfully" });
        }

        // DELETE /user/{id} (Admin only) - Delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
