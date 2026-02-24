using Microsoft.EntityFrameworkCore;
using JwtAuthDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Admin" },
                new Role { RoleId = 2, Name = "User" }
            );

            // Seed users with roles
            var hasher = new PasswordHasher<User>(); // Use ASP.NET Core's built-in password hasher to hash the passwords
            modelBuilder.Entity<User>().HasData( new User { 
                UserId = 1, Username = "admin", Password = hasher.HashPassword(null, "admin123"), RoleId = 1 }, 
                new User { UserId = 2, Username = "user", Password = hasher.HashPassword(null, "user123"), RoleId = 2 } 
            );
        }
    }
}
