using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class RegisterEmployeeDTO
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(15)]
        public string Phone { get; set; }

        public int? DepartmentId { get; set; }

        [MaxLength(255)]
        public string TechStack { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }

}
