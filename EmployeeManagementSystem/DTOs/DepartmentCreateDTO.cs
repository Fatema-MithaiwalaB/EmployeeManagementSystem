using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class DepartmentCreateDTO
    {
        [Required]
        public string DepartmentName { get; set; }
    }
}
