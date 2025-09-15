using System.Security.Claims;
using hechuyengia.Data;
using hechuyengia.Models;
using hechuyengia.Models.DTOs;
using hechuyengia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hechuyengia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwt;

        public AuthController(AppDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        // DTOs
        public record LoginDto(string Email, string Password);
        public record RegisterDto(string Email, string FullName, string Role, string Password);

        // ADMIN tạo user (Admin/Bác sĩ)
        [HttpPost("register")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            var u = new User
            {
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(u);
            await _db.SaveChangesAsync();

            if (dto.Role == "DOCTOR")
            {
                var doctor = new Doctor
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                };
                _db.Doctors.Add(doctor);
                await _db.SaveChangesAsync();
            }

            return Ok(new { u.UserId, u.Email, u.FullName, u.Role });
        }

        // Đăng nhập
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (u == null || !BCrypt.Net.BCrypt.Verify(dto.Password, u.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwt.CreateToken(u);
            return Ok(new { token, user = new { u.UserId, u.Email, u.FullName, u.Role, u.Phone } });
        }

        // Bác sĩ tự đăng ký (public)
        [HttpPost("register-doctor")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Thiếu thông tin");

            var email = dto.Email.Trim();
            if (await _db.Users.AnyAsync(x => x.Email == email))
                return BadRequest("Email đã tồn tại");

            var u = new User
            {
                Email = email,
                FullName = dto.FullName.Trim(),
                Role = "DOCTOR",
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(u);
            await _db.SaveChangesAsync();

            var doctor = new Doctor
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Specialty = dto.Specialty
            };
            _db.Doctors.Add(doctor);
            await _db.SaveChangesAsync();

            var token = _jwt.CreateToken(u);

            return Ok(new
            {
                token,
                user = new { u.UserId, u.Email, u.FullName, u.Role, u.Phone },
                doctorId = doctor.DoctorId
            });
        }

        // Lấy thông tin hiện tại
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("sub");
            if (!int.TryParse(idClaim, out var uid)) return Unauthorized();

            var u = await _db.Users.FirstOrDefaultAsync(x => x.UserId == uid);
            if (u == null) return NotFound();

            return Ok(new { u.UserId, u.Email, u.FullName, u.Role, u.Phone });
        }
    }
}
