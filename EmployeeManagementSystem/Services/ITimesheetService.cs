using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public interface ITimesheetService
    {
        Task<TimesheetResponseDTO> LogTimesheetAsync(TimesheetCreateDTO dto);
        Task<TimesheetResponseDTO?> GetTimesheetByIdAsync(int timesheetId);
        Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByEmployeeIdAsync(int employeeId);
        Task<TimesheetResponseDTO?> UpdateTimesheetAsync(int timesheetId, TimesheetCreateDTO dto);
        Task<bool> DeleteTimesheetAsync(int timesheetId);
        Task<IEnumerable<Timesheet>> GetAllTimesheetsAsync();
    }
}
