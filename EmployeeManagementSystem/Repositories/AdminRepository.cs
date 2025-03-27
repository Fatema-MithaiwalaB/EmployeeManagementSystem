using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin> GetAdminByIdAsync(int id) // ✅ Get Admin by ID
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _context.Admins
                .Include(a => a.Role)
                .Where(a => !a.isDeleted) // ✅ Exclude soft-deleted admins
                .ToListAsync();
        }

        public async Task AddAdminAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        public void UpdateAdmin(Admin admin) // ✅ Update Admin
        {
            _context.Admins.Update(admin);
        }

        public async Task<bool> SaveChangesAsync() // ✅ Save Changes
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
