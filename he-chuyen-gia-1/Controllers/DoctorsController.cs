// Controllers/DoctorsController.cs
using hechuyengia.Data;
using hechuyengia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hechuyengia.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DoctorsController(AppDbContext db) => _db = db;

        // DTO cho Doctor + User
        public class DoctorDto
        {
            public int DoctorId { get; set; }
            public string FullName { get; set; } = "";
            public string? Specialty { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
        }

        // Lấy danh sách bác sĩ (gồm Email + Phone từ User)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doctors = await _db.Doctors
                .Include(d => d.User)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Specialty = d.Specialty,
                    Email = d.User != null ? d.User.Email : null,
                    Phone = d.User != null ? d.User.Phone : null
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // Admin thêm bác sĩ mới → tạo User + Doctor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest("Thiếu thông tin");

            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email đã tồn tại");

            // Tạo User
            var user = new User
            {
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Phone = dto.Phone,
                Role = "DOCTOR",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") // mật khẩu mặc định
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Tạo Doctor
            var doctor = new Doctor
            {
                UserId = user.UserId,
                FullName = dto.FullName,
                Specialty = dto.Specialty
            };
            _db.Doctors.Add(doctor);
            await _db.SaveChangesAsync();

            return Ok(new { doctor.DoctorId, user.Email, user.Phone });
        }

        // Sửa thông tin bác sĩ (update Doctor + User)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto dto)
        {
            var doctor = await _db.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return NotFound();

            doctor.FullName = dto.FullName;
            doctor.Specialty = dto.Specialty;

            if (doctor.User != null)
            {
                doctor.User.FullName = dto.FullName;
                doctor.User.Email = dto.Email ?? doctor.User.Email;
                doctor.User.Phone = dto.Phone;
            }

            await _db.SaveChangesAsync();
            return Ok(new { doctor.DoctorId, dto.FullName, dto.Email, dto.Phone });
        }

        // Xoá bác sĩ (xóa luôn cả User liên quan)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _db.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return NotFound();

            if (doctor.User != null)
                _db.Users.Remove(doctor.User); // xóa cả tài khoản

            _db.Doctors.Remove(doctor);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
