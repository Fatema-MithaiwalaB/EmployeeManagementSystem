using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/leaves")]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveController(ILeaveService leaveService ,IEmployeeRepository employeeRepository)
        {
            _leaveService = leaveService;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("ApplyForLeave")]
        public async Task<IActionResult> ApplyForLeave([FromBody] LeaveCreateDTO dto)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (loggedInUserId != dto.EmployeeId)
                return Forbid();

            var result = await _leaveService.ApplyForLeaveAsync(dto);

            if (result == null)
                return BadRequest(new { message = "Employee not found." });

            if (result.Reason == "Start date cannot be after the end date." ||
                result.Reason == "You already have a leave request for this period.")
                return BadRequest(new { message = result.Reason });

            return Ok(result);
        }

        [HttpGet("GetLeaveById/{id}")]
        public async Task<IActionResult> GetLeaveById(int id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);

            if (leave == null) return NotFound();

            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Admin")
                return Ok(leave);

            if (leave.EmployeeId != loggedInUserId)
                return Forbid();

            return Ok(leave);
        }


        [HttpPut("UpdateLeaveStatus/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] string status)
        {
            var result = await _leaveService.UpdateLeaveStatusAsync(id, status);

            if (!result)
                return NotFound(new { message = "Leave request not found or could not be updated." });

            return Ok(new { message = "Leave status updated successfully." });
        }


        [HttpDelete("DeleteLeave/{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                var leave = await _leaveService.GetLeaveByIdAsync(id);
                if (leave == null)
                    return NotFound(new { message = "Leave request not found." });

                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                // Only allow the employee who requested it or an admin to delete
                if (userRole != "Admin" && leave.EmployeeId != loggedInUserId)
                    return Forbid();

                if (leave.Status != "Pending")
                    return BadRequest(new { message = "Cannot delete a leave request that has already been processed." });

                await _leaveService.DeleteLeaveAsync(id);
                return Ok(new { message = "Leave request deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the leave request." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPendingLeaves")]
        public async Task<IActionResult> GetPendingLeaves(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pendingLeaves = await _leaveService.GetPendingLeavesAsync(pageNumber, pageSize);

                if (!pendingLeaves.Any())
                    return Ok(new { message = "No pending leave requests found.", leaves = pendingLeaves });

                return Ok(pendingLeaves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching pending leaves." });
            }
        }

        [HttpGet("GetLeavesByEmployee/{employeeId}")]
        public async Task<IActionResult> GetLeavesByEmployee(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return NotFound(new { message = "Employee not found." });

            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin" && loggedInUserId != employeeId)
                return Forbid();

            var leaves = await _leaveService.GetLeavesByEmployeeIdAsync(employeeId);

            if (!leaves.Any())
                return Ok(new { message = "No leave records found for this employee.", leaves = leaves });

            return Ok(leaves);
        }



    }
}
