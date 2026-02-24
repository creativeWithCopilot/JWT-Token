namespace JwtAuthDemo.Models.DTOs
{
    // For reading user data (without password)
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
    }

    // For creating a new user
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    // For updating an existing user
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
