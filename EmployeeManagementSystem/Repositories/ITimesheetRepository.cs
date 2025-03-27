using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public interface ITimesheetRepository
    {
        Task<Timesheet?> GetByIdAsync(int id);
        Task<List<Timesheet>> GetByDateAsync(int employeeId, DateTime date);
        Task<List<Timesheet>> GetByEmployeeIdAsync(int employeeId);
        Task AddAsync(Timesheet timesheet);
        Task UpdateAsync(Timesheet timesheet);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Timesheet>> GetAllTimesheetsAsync();
    }
}
