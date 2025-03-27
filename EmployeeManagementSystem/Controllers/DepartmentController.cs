using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Department
        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }

        // GET: api/Department/{id}
        [HttpGet("GeDepartmenttById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            var response = new DepartmentResponseDTO
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };

            return Ok(response);
        }

        // POST: api/Department
        [HttpPost("CreateDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newDepartment = await _departmentService.CreateAsync(dto);
            var response = new DepartmentResponseDTO
            {
                DepartmentId = newDepartment.DepartmentId,
                DepartmentName = newDepartment.DepartmentName
            };

            return CreatedAtAction(nameof(GetById), new { id = response.DepartmentId }, new
            {
                message = "Department created successfully",
                department = response
            });
        }

        // PUT: api/Department/{id}
        [HttpPut("UpdateDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDepartment = await _departmentService.UpdateAsync(id, dto);
            if (updatedDepartment == null)
                return NotFound(new { message = "Department not found" });
            if(updatedDepartment == true)
            {
                return Ok(new
                {
                    message = "Department updated successfully",
                    department = updatedDepartment
                });
            }
            else
            {
                return Ok(new
                {
                    message = "Department is not updated",
                    department = updatedDepartment
                });
            }
            
        }

        // DELETE: api/Department/{id}
        [HttpDelete("DeleteDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Department not found" });

            return Ok(new { message = "Department deleted successfully" });
        }
    }
}
