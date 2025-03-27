using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class EmployeeAuthController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public EmployeeAuthController(IEmployeeRepository employeeRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _employeeRepository = employeeRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // ✅ Employee Signup (Register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegisterDTO dto)
        {
            if (await _employeeRepository.GetByEmailAsync(dto.Email) != null)
            {
                return BadRequest(new { message = "Email already in use" });
            }

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                TechStack = dto.TechStack,
                Address = dto.Address,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                PasswordHash = PasswordHasher.HashPassword(dto.Password)
            };

            await _employeeRepository.AddAsync(employee);

            return Ok(new { message = "Employee registered successfully" });
        }

        // ✅ Employee Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeeLoginDTO dto)
        {
            var employee = await _employeeRepository.GetByEmailAsync(dto.Email);
            if (employee == null || !PasswordHasher.VerifyPassword(dto.Password, employee.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _jwtTokenGenerator.GenerateToken(employee.EmployeeId, employee.Email, employee.RoleId);
            return Ok(new EmployeeResponseDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                RoleId = employee.RoleId,
                Token = token
            });
        }
    }
}
