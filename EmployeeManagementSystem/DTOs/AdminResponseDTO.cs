namespace EmployeeManagementSystem.DTOs
{
    public class AdminResponseDTO
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int RoleId { get; set; }
    }
}
