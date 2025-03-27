using EmployeeManagementSystem.Models;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByEmailAsync(string email); // ✅ Get Admin by Email
        Task<Admin> GetAdminByIdAsync(int id); // ✅ Get Admin by ID
        Task AddAdminAsync(Admin admin); // ✅ Add Admin
        void UpdateAdmin(Admin admin); // ✅ Update Admin
        Task<bool> SaveChangesAsync(); // ✅ Save changes in DB
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
    }
}
