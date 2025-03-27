using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public interface ILeaveRepository
    {
        Task<Leave> ApplyLeaveAsync(Leave leave);
        Task<Leave> GetLeaveByIdAsync(int leaveId);
        Task<IEnumerable<Leave>> GetLeavesByEmployeeIdAsync(int employeeId);
        Task<Leave> GetOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<bool> UpdateLeaveStatusAsync(int leaveId, string status);
        Task<bool> DeleteLeaveAsync(int leaveId);
        Task<IEnumerable<Leave>> GetPendingLeavesPaginatedAsync(int pageNumber, int pageSize);
    }
}
