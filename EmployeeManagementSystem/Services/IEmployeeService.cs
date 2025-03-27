using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using System.Security.Claims;

public interface IEmployeeService
{
    Task<string> CreateEmployeeAsync(EmployeeRegisterDTO dto);
    Task<Employee?> GetEmployeeByIdAsync(int id, ClaimsPrincipal user);
    Task<string> UpdateEmployeeAsync(int id, EmployeeRegisterDTO dto, ClaimsPrincipal user);
    Task<string> DeleteEmployeeAsync(int id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<string> RegisterAsync(EmployeeRegisterDTO dto);
    Task<EmployeeResponseDTO?> LoginAsync(EmployeeLoginDTO dto);
}