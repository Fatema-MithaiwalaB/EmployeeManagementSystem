using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task<Department> CreateAsync(Department department);
        Task<bool> UpdateAsync(Department department);
        Task<bool> DeleteAsync(int id);
    }
}
