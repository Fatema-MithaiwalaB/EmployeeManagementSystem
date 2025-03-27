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
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);

            if (existingEmployee == null)
            {
                _logger.LogWarning($"Employee with ID {employee.EmployeeId} not found.");
                return;
            }

            // ✅ Update properties manually
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.TechStack = employee.TechStack;
            existingEmployee.Address = employee.Address;
            existingEmployee.DepartmentId = employee.DepartmentId;

            _context.Employees.Attach(existingEmployee);
            _context.Entry(existingEmployee).State = EntityState.Modified;  // ✅ Mark entity as modified

            int changes = await _context.SaveChangesAsync();
            _logger.LogInformation($"Employee updated successfully. Rows affected: {changes}");
        }

    }
}
