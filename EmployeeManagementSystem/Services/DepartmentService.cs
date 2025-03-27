using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepo;

        public DepartmentService(IDepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public async Task<IEnumerable<DepartmentResponseDTO>> GetAllAsync()
        {
            var departments = await _departmentRepo.GetAllAsync();
            return departments.Select(d => new DepartmentResponseDTO
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            });
        }

        public async Task<DepartmentResponseDTO> GetByIdAsync(int id)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentResponseDTO
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };
        }

        public async Task<DepartmentResponseDTO> CreateAsync(DepartmentCreateDTO dto)
        {
            var newDepartment = new Department
            {
                DepartmentName = dto.DepartmentName
            };

            var createdDepartment = await _departmentRepo.CreateAsync(newDepartment);

            return new DepartmentResponseDTO
            {
                DepartmentId = createdDepartment.DepartmentId,
                DepartmentName = createdDepartment.DepartmentName
            };
        }

        public async Task<bool> UpdateAsync(int id, DepartmentCreateDTO dto)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null) return false; // Not found

            department.DepartmentName = dto.DepartmentName;

            // Ensure changes are saved
            bool isUpdated = await _departmentRepo.UpdateAsync(department);
            return isUpdated;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await _departmentRepo.DeleteAsync(id);
        }
    }
}
