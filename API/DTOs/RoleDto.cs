namespace JwtAuthDemo.Models.DTOs
{
    // For reading role data
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }

    // For creating a new role
    public class CreateRoleDto
    {
        public string Name { get; set; }
    }

    // For updating an existing role
    public class UpdateRoleDto
    {
        public string Name { get; set; }
    }
}
