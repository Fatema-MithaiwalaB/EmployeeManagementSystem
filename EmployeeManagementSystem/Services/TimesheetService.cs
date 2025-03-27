using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<TimesheetResponseDTO> LogTimesheetAsync(TimesheetCreateDTO dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new Exception("Employee not found.");

            var existingTimesheet = await _timesheetRepository.GetByDateAsync(dto.EmployeeId, dto.Date);
            if (existingTimesheet.Any())
                throw new Exception("Timesheet already exists for this date.");

            if (dto.StartTime >= dto.EndTime)
                throw new Exception("Start Time cannot be greater than or equal to End Time.");

            var totalHours = (dto.EndTime - dto.StartTime).TotalHours;
            if (totalHours > 11)
                throw new Exception("Total work hours cannot exceed 11 hours.");

            var timesheet = new Timesheet
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TotalHours = (decimal)totalHours,
                Description = dto.Description
            };

            await _timesheetRepository.AddAsync(timesheet);

            return new TimesheetResponseDTO
            {
                TimesheetId = timesheet.TimesheetId,
                EmployeeId = timesheet.EmployeeId,
                Date = timesheet.Date,
                StartTime = timesheet.StartTime,
                EndTime = timesheet.EndTime,
                TotalHours = timesheet.TotalHours,
                Description = timesheet.Description
            };
        }

        public async Task<TimesheetResponseDTO?> GetTimesheetByIdAsync(int timesheetId)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(timesheetId);
            return timesheet == null ? null : new TimesheetResponseDTO
            {
                TimesheetId = timesheet.TimesheetId,
                EmployeeId = timesheet.EmployeeId,
                Date = timesheet.Date,
                StartTime = timesheet.StartTime,
                EndTime = timesheet.EndTime,
                TotalHours = timesheet.TotalHours,
                Description = timesheet.Description
            };
        }

        public async Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByEmployeeIdAsync(int employeeId)
        {
            var timesheets = await _timesheetRepository.GetByEmployeeIdAsync(employeeId);
            return timesheets.Select(t => new TimesheetResponseDTO
            {
                TimesheetId = t.TimesheetId,
                EmployeeId = t.EmployeeId,
                Date = t.Date,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                TotalHours = t.TotalHours,
                Description = t.Description
            });
        }

        public async Task<TimesheetResponseDTO?> UpdateTimesheetAsync(int timesheetId, TimesheetCreateDTO dto)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(timesheetId);
            if (timesheet == null)
                return null;

            timesheet.Date = dto.Date;
            timesheet.StartTime = dto.StartTime;
            timesheet.EndTime = dto.EndTime;
            timesheet.TotalHours = (decimal)(dto.EndTime - dto.StartTime).TotalHours;
            timesheet.Description = dto.Description;

            await _timesheetRepository.UpdateAsync(timesheet);

            return new TimesheetResponseDTO
            {
                TimesheetId = timesheet.TimesheetId,
                EmployeeId = timesheet.EmployeeId,
                Date = timesheet.Date,
                StartTime = timesheet.StartTime,
                EndTime = timesheet.EndTime,
                TotalHours = timesheet.TotalHours,
                Description = timesheet.Description
            };
        }

        public async Task<bool> DeleteTimesheetAsync(int timesheetId)
        {
            return await _timesheetRepository.DeleteAsync(timesheetId);
        }

        public async Task<IEnumerable<Timesheet>> GetAllTimesheetsAsync()
        {
            return await _timesheetRepository.GetAllTimesheetsAsync();
        }

    }
}
