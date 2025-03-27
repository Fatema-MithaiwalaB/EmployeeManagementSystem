using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class AdminRegisterDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } // Will be hashed

        [Required]
        public string Phone { get; set; }

        [Required]
        public int RoleId { get; set; } // 2 = Admin
    }
}
