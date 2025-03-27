using BCrypt.Net;

namespace EmployeeManagementSystem.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            Console.WriteLine($"Entered Password: {password}");
            Console.WriteLine($"Stored Hashed Password: {hashedPassword}");

            bool isMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            Console.WriteLine($"Password Match: {isMatch}");
            return isMatch;
        }

    }
}
