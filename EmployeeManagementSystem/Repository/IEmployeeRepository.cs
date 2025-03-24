using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync (Employee employee);
        Task<bool> UpdateEmployeeAsync (Employee employee);
        Task<bool> DeleteEmployeeAsync (int id);
    }
}
