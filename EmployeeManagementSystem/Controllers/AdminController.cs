﻿using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminRepository adminRepository, ILogger<AdminController> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        // Create Admin
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminRegisterDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { message = "Invalid request data." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _adminRepository.GetAdminByEmailAsync(dto.Email) != null)
                    return BadRequest(new { message = "Admin with this email already exists." });

                if (dto.RoleId == 1)
                    return BadRequest(new { message = "You are trying to create an Employee in Admin login. Please use role ID = 2." });

                var admin = new Admin
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Phone = dto.Phone,
                    RoleId = 2,
                    isDeleted = false
                };

                await _adminRepository.AddAdminAsync(admin);
                await _adminRepository.SaveChangesAsync(); // Ensure this is called

                return Ok(new { message = "Admin created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating admin: {ex.Message}");
                return StatusCode(500, new { message = $"An error occurred while creating the admin: {ex.Message}" });
            }
        }

        [HttpGet("all-admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _adminRepository.GetAllAdminsAsync();
                var adminDtos = admins
                    .Where(a => !a.isDeleted)
                    .Select(a => new AdminResponseDTO
                    {
                        AdminId = a.AdminId,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Email = a.Email,
                        RoleId = a.RoleId
                    }).ToList();

                return Ok(adminDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admins.");
                return StatusCode(500, new { message = "An error occurred while retrieving admins." });
            }
        }

        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(id);
                if (admin == null || admin.isDeleted)
                    return NotFound(new { message = "Admin not found" });

                return Ok(new AdminResponseDTO
                {
                    AdminId = admin.AdminId,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    RoleId = admin.RoleId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admin with ID {AdminId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the admin." });
            }
        }

        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody] AdminRegisterDTO dto)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(id);
                if (admin == null || admin.isDeleted)
                    return NotFound(new { message = "Admin not found" });

                var loggedInAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (loggedInAdminId != id)
                    return Forbid();

                var existingAdmin = await _adminRepository.GetAdminByEmailAsync(dto.Email);
                if (existingAdmin != null && existingAdmin.AdminId != id)
                {
                    return BadRequest(new { message = "Admin with this email already exists." });
                }


                admin.FirstName = dto.FirstName;
                admin.LastName = dto.LastName;
                admin.Phone = dto.Phone;
                admin.Email = dto.Email;

                _adminRepository.UpdateAdmin(admin);
                await _adminRepository.SaveChangesAsync();

                return Ok(new { message = "Admin updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating admin with ID {AdminId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the admin." });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> SoftDeleteAdmin(int id)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(id);
                if (admin == null || admin.isDeleted)
                    return NotFound(new { message = "Admin not found" });

                admin.isDeleted = true;
                _adminRepository.UpdateAdmin(admin);
                await _adminRepository.SaveChangesAsync();

                return Ok(new { message = "Admin soft deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting admin with ID {AdminId}", id);
                return StatusCode(500, new { message = "An error occurred while soft deleting the admin." });
            }
        }

        // ✅ Restore Soft Deleted Admin
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreAdmin(int id)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(id);
                if (admin == null || !admin.isDeleted)
                    return NotFound(new { message = "Admin not found or not deleted." });

                admin.isDeleted = false;
                _adminRepository.UpdateAdmin(admin);
                await _adminRepository.SaveChangesAsync();

                return Ok(new { message = "Admin restored successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring admin with ID {AdminId}", id);
                return StatusCode(500, new { message = "An error occurred while restoring the admin." });
            }
        }
    }
}
