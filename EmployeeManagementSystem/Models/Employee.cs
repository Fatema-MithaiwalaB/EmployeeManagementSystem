using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // Unique & Required
        public string Phone { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string TechStack { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool isDeleted { get; set; } = false;

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    }
}
