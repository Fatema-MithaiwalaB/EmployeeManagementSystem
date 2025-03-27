using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ApplicationDbContext context, ILogger<EmployeeRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            _logger.LogInformation($"Fetching Employee by Email: {email}");
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveAsync()
        {
            _logger.LogInformation("Fetching all active employees.");
            return await _context.Employees
                .Where(e => !e.isDeleted)
                .ToListAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (existingEmployee == null)
            {
                _logger.LogWarning($"Employee with ID {employee.EmployeeId} not found.");
                return;
            }

            // Log the old and new email
            _logger.LogInformation($"Existing Email: {existingEmployee.Email}, New Email: {employee.Email}");

            // Check if any property has actually changed
            if (existingEmployee.Email == employee.Email &&
                existingEmployee.FirstName == employee.FirstName &&
                existingEmployee.LastName == employee.LastName &&
                existingEmployee.Phone == employee.Phone &&
                existingEmployee.TechStack == employee.TechStack &&
                existingEmployee.Address == employee.Address &&
                existingEmployee.DepartmentId == employee.DepartmentId)
            {
                _logger.LogWarning($"No changes detected for Employee ID {employee.EmployeeId}. Update skipped.");
                return;
            }

            _context.Employees.Update(employee);
            int changes = await _context.SaveChangesAsync();

            _logger.LogInformation($"Rows affected: {changes} for Employee ID {employee.EmployeeId}");
        }

    }
}
