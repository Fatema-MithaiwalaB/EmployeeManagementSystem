﻿namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeResponseDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
    }
}
