// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace hechuyengia.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string Role { get; set; } = "DOCTOR"; // ADMIN, DOCTOR

        public string FullName { get; set; } = "";

        // 👇 Thêm số điện thoại
        public string? Phone { get; set; }

        // Option: map sang Doctor (1-1)
        public Doctor? Doctor { get; set; }
    }
}
