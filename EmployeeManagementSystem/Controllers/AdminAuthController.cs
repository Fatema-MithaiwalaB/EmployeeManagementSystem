using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BCrypt.Net;
using EmployeeManagementSystem.Helpers;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;
        private readonly JwtTokenGenerator _tokenService;

        public AdminAuthController(IAdminRepository adminRepo, JwtTokenGenerator tokenService)
        {
            _adminRepo = adminRepo;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminRegisterDTO dto)
        {
            var existingAdmin = await _adminRepo.GetAdminByEmailAsync(dto.Email);
            if (existingAdmin != null)
                return BadRequest("Admin already exists.");

            if (dto.RoleId == 1)
                return BadRequest(new { message = "Please use role id as 2." });

            var admin = new Admin
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                RoleId = dto.RoleId
            };

            await _adminRepo.AddAdminAsync(admin);

            var token = _tokenService.GenerateToken(admin.AdminId, admin.Email, admin.RoleId);


            return Ok(new AdminResponseDTO
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Token = token,
                RoleId = admin.RoleId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDTO dto)
        {
            var admin = await _adminRepo.GetAdminByEmailAsync(dto.Email);
            if (admin == null || !PasswordHasher.VerifyPassword(dto.Password, admin.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.GenerateToken(admin.AdminId, admin.Email, admin.RoleId);


            return Ok(new AdminResponseDTO
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Token = token,
                RoleId = admin.RoleId
            });
        }
    }
}
