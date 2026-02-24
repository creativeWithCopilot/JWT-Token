namespace JwtAuthDemoUI.Models
{
    // Data Transfer Object used when sending login requests.
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Response returned by the backend after a successful login. Contains the JWT token and user details.
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

    // Represents a user in the system. Contains basic information such as user ID, username, and role.
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}