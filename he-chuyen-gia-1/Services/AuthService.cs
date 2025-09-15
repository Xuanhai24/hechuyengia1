using hechuyengia.Models;
using hechuyengia.Auth;

namespace hechuyengia.Services
{
    public interface IAuthService
    {
        User? ValidateUser(string email, string password);
        IEnumerable<User> GetAll();
    }

    public class AuthService : IAuthService
    {
        // Demo in-memory. Sản xuất hãy thay bằng EF Core/DB.
        private static readonly List<User> Users = new()
        {
            new User {
                UserId = 1,
                Email = "admin@local",
                FullName = "Quản trị",
                Role = "ADMIN",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
            },
            new User {
                UserId = 2,
                Email = "bacsi@local",
                FullName = "Bác sĩ A",
                Role = "DOCTOR",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doctor@123")
            }
        };

        public User? ValidateUser(string email, string password)
        {
            var user = Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user is null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

            // Trả về user rút gọn, không kèm PasswordHash
            return new User
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                PasswordHash = string.Empty
            };
        }

        public IEnumerable<User> GetAll() =>
            Users.Select(u => new User
            {
                UserId = u.UserId,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role,
                PasswordHash = string.Empty
            });
    }
}
