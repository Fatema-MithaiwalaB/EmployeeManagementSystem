using EmployeeManagementSystem.DTOs;

namespace EmployeeManagementSystem.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDTO>> GetAllAsync();
        Task<DepartmentResponseDTO> GetByIdAsync(int id);
        Task<DepartmentResponseDTO> CreateAsync(DepartmentCreateDTO dto);
        Task<bool> UpdateAsync(int id, DepartmentCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
