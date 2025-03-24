namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeCreateOrUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TechStack { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }

}
