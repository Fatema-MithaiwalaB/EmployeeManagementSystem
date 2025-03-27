using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Leave> ApplyLeaveAsync(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();
            return leave;
        }

        public async Task<Leave> GetLeaveByIdAsync(int leaveId)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.LeaveId == leaveId);
        }

        public async Task<IEnumerable<Leave>> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<Leave> GetOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId &&
                            l.StartDate <= endDate &&
                            l.EndDate >= startDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateLeaveStatusAsync(int leaveId, string status)
        {
            var leave = await GetLeaveByIdAsync(leaveId);
            if (leave == null) return false;

            leave.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLeaveAsync(int leaveId)
        {
            var leave = await GetLeaveByIdAsync(leaveId);
            if (leave == null) return false;

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Leave>> GetPendingLeavesPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Leaves
                .Where(l => l.Status == "Pending")
                .OrderBy(l => l.StartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
