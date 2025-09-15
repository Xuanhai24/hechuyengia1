// Controllers/ReportsController.cs
using hechuyengia.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hechuyengia.Controllers
{
    [Authorize(Roles = "DOCTOR,ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ReportsController(AppDbContext db) => _db = db;

        [HttpGet("counts")]
        public async Task<IActionResult> Counts()
        {
            var patients = await _db.Patients.CountAsync();
            var doctors = await _db.Doctors.CountAsync();
            var diag = await _db.Diagnoses.CountAsync();
            return Ok(new { patients, doctors, diagnoses = diag });
        }

        [HttpGet("diagnoses-by-date")]
        public async Task<IActionResult> DiagnosesByDate(DateTime? from, DateTime? to)
        {
            var start = from ?? DateTime.UtcNow.AddDays(-30);
            var end = (to ?? DateTime.UtcNow).Date.AddDays(1);

            var data = await _db.Diagnoses
                .Where(d => d.CreatedAt >= start && d.CreatedAt < end)
                .GroupBy(d => d.CreatedAt.Date)
                .Select(g => new { date = g.Key, count = g.Count() })
                .OrderBy(x => x.date)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("top-diseases")]
        public async Task<IActionResult> TopDiseases(int take = 5)
        {
            var data = await _db.Diagnoses
                .Where(d => d.Result != null && d.Result != "")
                .GroupBy(d => d.Result!)
                .Select(g => new { disease = g.Key, count = g.Count() })
                .OrderByDescending(x => x.count)
                .Take(take)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("doctor-workload")]
        public async Task<IActionResult> DoctorWorkload()
        {
            var data = await _db.Diagnoses
                .GroupBy(d => d.Doctor.FullName)
                .Select(g => new { doctor = g.Key, count = g.Count() })
                .OrderByDescending(x => x.count)
                .ToListAsync();

            return Ok(data);
        }
    }
}
