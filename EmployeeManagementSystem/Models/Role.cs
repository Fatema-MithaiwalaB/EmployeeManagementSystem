using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; } // "Employee" or "Admin"

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
    }
}
