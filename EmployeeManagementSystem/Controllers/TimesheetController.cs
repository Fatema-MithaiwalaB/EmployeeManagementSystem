using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using OfficeOpenXml;


namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/timesheets")]
    [Authorize] // Requires authentication for all endpoints
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;
        private readonly ILogger<TimesheetController> _logger;

        public TimesheetController(ITimesheetService timesheetService, ILogger<TimesheetController> logger)
        {
            _timesheetService = timesheetService;
            _logger = logger;
        }

        // ✅ Log a new timesheet entry
        [HttpPost("CreateTimesheet")]
        public async Task<IActionResult> LogTimesheet([FromBody] TimesheetCreateDTO dto)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Ensure employee is logging their own timesheet
                if (loggedInUserId != dto.EmployeeId)
                    return Forbid();

                if (dto.Date.Date > (DateTime.UtcNow.Date))
                {
                    return BadRequest(new { message = "Timesheets cannot be created for future dates." });
                }

                var timesheet = await _timesheetService.LogTimesheetAsync(dto);
                return CreatedAtAction(nameof(GetTimesheetById), new { id = timesheet.TimesheetId }, timesheet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging timesheet");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Get timesheet by ID
        [HttpGet("GetTimesheetById/{id}")]
        public async Task<IActionResult> GetTimesheetById(int id)
        {
            try
            {
                var timesheet = await _timesheetService.GetTimesheetByIdAsync(id);
                if (timesheet == null)
                    return NotFound(new { message = "Timesheet not found" });

                return Ok(timesheet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching timesheet");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // ✅ Get all timesheets for an employee
        [HttpGet("GetTimesheetsByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetTimesheetsByEmployeeId(int employeeId)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                // Employees can only access their own timesheets, Admins can access all
                if (userRole != "Admin" && loggedInUserId != employeeId)
                    return Forbid();

                var timesheets = await _timesheetService.GetTimesheetsByEmployeeIdAsync(employeeId);
                return Ok(timesheets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching timesheets");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // ✅ Update a timesheet entry
        [HttpPut("UpdateTimesheet/{id}")]
        public async Task<IActionResult> UpdateTimesheet(int id, [FromBody] TimesheetCreateDTO dto)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var timesheet = await _timesheetService.GetTimesheetByIdAsync(id);
                if (timesheet == null)
                    return NotFound(new { message = "Timesheet not found" });

                // Ensure employee is updating their own timesheet
                if (loggedInUserId != timesheet.EmployeeId)
                    return Forbid();

                var updatedTimesheet = await _timesheetService.UpdateTimesheetAsync(id, dto);
                return Ok(updatedTimesheet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating timesheet");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Delete a timesheet entry
        [Authorize] 
        [HttpDelete("DeleteTimesheet/{id}")]
        public async Task<IActionResult> DeleteTimesheet(int id)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role); // Get user role

                var timesheet = await _timesheetService.GetTimesheetByIdAsync(id);
                if (timesheet == null)
                    return NotFound(new { message = "Timesheet not found" });

                // ✅ Allow Admins to delete any timesheet
                if (userRole == "Admin")
                {
                    var result = await _timesheetService.DeleteTimesheetAsync(id);
                    if (!result)
                        return StatusCode(500, new { message = "Error deleting timesheet" });

                    return Ok(new { message = "Timesheet deleted successfully" });
                }

                return Forbid(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting timesheet");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }


        [HttpGet("ExportTimesheets")]
        [Authorize(Roles = "Admin")]
        // ✅ Export All Employee Timesheets to Excel
        public async Task<IActionResult> DownloadEmployeeTimesheets()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var timesheets = await _timesheetService.GetAllTimesheetsAsync();
                if (!timesheets.Any()) return NotFound(new { message = "No timesheets found." });

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Timesheets");

                worksheet.Cells["A1"].Value = "Employee ID";
                worksheet.Cells["B1"].Value = "Date";
                worksheet.Cells["C1"].Value = "Start Time";
                worksheet.Cells["D1"].Value = "End Time";
                worksheet.Cells["E1"].Value = "Total Hours";
                worksheet.Cells["F1"].Value = "Description";

                int row = 2;
                foreach (var timesheet in timesheets)
                {
                    worksheet.Cells[row, 1].Value = timesheet.EmployeeId;
                    worksheet.Cells[row, 2].Value = timesheet.Date.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 3].Value = timesheet.StartTime.ToString();
                    worksheet.Cells[row, 4].Value = timesheet.EndTime.ToString();
                    worksheet.Cells[row, 5].Value = timesheet.TotalHours;
                    worksheet.Cells[row, 6].Value = timesheet.Description;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee_Timesheets.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting timesheets.");
                return StatusCode(500, new { message = "An error occurred while exporting timesheets. Please try again later." });
            }
        }

    }

}

