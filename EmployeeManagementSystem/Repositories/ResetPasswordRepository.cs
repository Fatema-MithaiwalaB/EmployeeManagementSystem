using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class ResetPasswordRepository
    {
        private readonly ApplicationDbContext _context;

        public ResetPasswordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task StorePasswordResetTokenAsync(Employee user, string token)
        {
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Employees.SingleOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null; // Clear token after reset
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
