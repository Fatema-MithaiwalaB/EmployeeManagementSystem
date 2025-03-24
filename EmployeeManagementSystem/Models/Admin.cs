using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }

}
