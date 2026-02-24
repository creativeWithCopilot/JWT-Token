namespace JwtAuthDemoUI.Models
{
    // Response returned by the /roletest/profile endpoint. Contains the user's profile information such as username and role.
    public class ProfileResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
