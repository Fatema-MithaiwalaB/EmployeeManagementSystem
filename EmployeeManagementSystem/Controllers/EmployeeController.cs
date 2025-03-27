using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public EmployeeController(IEmployeeRepository employeeRepository, IAdminRepository adminRepository, ILogger<EmployeeController> logger, ApplicationDbContext context, IEmailService emailService)
        {
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("CreateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRegisterDTO dto)
        {
            try
            {
                if (dto.RoleId == 2)
                    return BadRequest(new { message = "Please use role id as 1." });
                var newEmployee = new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Phone = dto.Phone,
                    TechStack = dto.TechStack,
                    Address = dto.Address,
                    DepartmentId = dto.DepartmentId,
                    RoleId = 1, 
                    isDeleted = false
                };

                await _employeeRepository.AddAsync(newEmployee);
                await _employeeRepository.SaveChangesAsync();

                return Ok(new { message = "Employee registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering employee.");
                return StatusCode(500, new { message = "An error occurred while creating the employee." });
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null || employee.isDeleted)
                    return NotFound(new { message = "Employee not found" });

                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "Admin" && loggedInUserId != id)
                    return Forbid();

                var employeeDto = new EmployeeResponseDTO
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    RoleId = employee.RoleId,
                    Token = null
                };

                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }


        // ✅ UPDATE Employee (Self or Admin Only)
        [HttpPut("UpdateEmployee/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeRegisterDTO dto)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null || employee.isDeleted)
                    return NotFound(new { message = "Employee not found" });

                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "Admin" && loggedInUserId != id)
                    return Forbid();

                
                var updatedEmployee = new Employee
                {
                    EmployeeId = employee.EmployeeId, 
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    TechStack = dto.TechStack,
                    Address = dto.Address,
                    DepartmentId = dto.DepartmentId,
                    isDeleted = employee.isDeleted 
                };

                await _employeeRepository.UpdateAsync(updatedEmployee);
                return Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }


        [HttpDelete("DeleteEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null || employee.isDeleted)
                    return NotFound(new { message = "Employee not found" });

                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (loggedInUserId == id)
                    return Forbid("You cannot deactivate your own account");

                employee.isDeleted = true;
                _context.Employees.Update(employee); 
                _context.Entry(employee).Property(e => e.isDeleted).IsModified = true; 
                await _context.SaveChangesAsync();


                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

       
        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employee = await _employeeRepository.GetAllActiveAsync();
                var employeeDtos = employee.Select(employee => new EmployeeResponseDTO
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    RoleId = employee.RoleId,
                    Token = null 
                }).ToList();

                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        [HttpPut("RestoreEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null || !employee.isDeleted)
                    return NotFound(new { message = "Employee not found" });

                employee.isDeleted = false;
                _context.Employees.Update(employee); 
                _context.Entry(employee).Property(e => e.isDeleted).IsModified = true; 
                await _context.SaveChangesAsync();
                return Ok(new { message = "Employee restored successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessageDTO emailMessage)
        {
            await _emailService.SendEmailAsync(emailMessage);
            return Ok("Email sent successfully.");
        }
    }
}