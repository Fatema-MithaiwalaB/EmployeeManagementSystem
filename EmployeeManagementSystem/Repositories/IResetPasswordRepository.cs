using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repositories
{
    public interface IResetPasswordRepository
    {
        Task<Employee> GetByEmailAsync(string email);
        Task StorePasswordResetTokenAsync(Employee user, string token);
        Task<bool> ResetPasswordAsync(string token, string newPassword);

    }
}
