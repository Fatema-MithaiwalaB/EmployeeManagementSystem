using EmployeeManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public interface ILeaveService
    {
        Task<LeaveResponseDTO> ApplyForLeaveAsync(LeaveCreateDTO dto);
        Task<LeaveResponseDTO> GetLeaveByIdAsync(int leaveId);
        Task<IEnumerable<LeaveResponseDTO>> GetLeavesByEmployeeIdAsync(int employeeId);
        Task<bool> UpdateLeaveStatusAsync(int leaveId, string status);
        Task<bool> DeleteLeaveAsync(int leaveId);
        Task<IEnumerable<LeaveResponseDTO>> GetPendingLeavesAsync(int pageNumber, int pageSize);
    }
}
