using System.ComponentModel.DataAnnotations;

namespace JwtAuthDemo.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property for related users
        public ICollection<User> Users { get; set; }
    }
}
