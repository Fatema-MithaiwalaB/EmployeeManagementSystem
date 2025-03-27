using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetAllActiveAsync();
        Task UpdateAsync(Employee employee);  
    }
}
