using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, IAdminRepository adminRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;
            _logger = logger;
        }

        // ✅ CREATE Employee (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRegisterDTO dto)
        {
            try
            {
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
                    RoleId = 1, // Default role (Employee)
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



        // ✅ GET Employee by ID (Self or Admin Only)
        [HttpGet("{id}")]
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

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // ✅ UPDATE Employee (Self or Admin Only)
        [HttpPut("{id}")]
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

                employee.FirstName = dto.FirstName;
                employee.LastName = dto.LastName;
                employee.Email = dto.Email;
                employee.Phone = dto.Phone;
                employee.TechStack = dto.TechStack;
                employee.Address = dto.Address;
                employee.DepartmentId = dto.DepartmentId;

                await _employeeRepository.UpdateAsync(employee);
                return Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // ✅ DELETE Employee (Admin Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null || employee.isDeleted)
                    return NotFound(new { message = "Employee not found" });

                employee.isDeleted = true;
                await _employeeRepository.UpdateAsync(employee);
                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }

        // ✅ GET ALL Employees (Admin Only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllActiveAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }
    }
}
