// Controllers/PatientsController.cs
using hechuyengia.Data;
using hechuyengia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hechuyengia.Controllers
{
    [Authorize(Roles = "DOCTOR,ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PatientsController(AppDbContext db) => _db = db;

        [HttpGet("Search")]
        public async Task<IActionResult> SearchById([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Vui lòng nhập Mã bệnh nhân");

            if (!int.TryParse(id, out int patientId))
                return BadRequest("Mã bệnh nhân phải là số hợp lệ");

            var patients = await _db.Patients
                .Where(p => p.PatientId.ToString().Contains(id))
                .AsNoTracking()
                .ToListAsync();

            if (patients == null || patients.Count == 0)
                return NotFound();
            return Ok(patients);
        }

        // GET /api/Patients?search=&sort=name|dob|name_desc|dob_desc
        [HttpGet]
        public async Task<IActionResult> Get(string? search, string sort = "name")
        {
            var q = _db.Patients.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.FullName.Contains(search));

            q = sort switch
            {
                "dob" => q.OrderBy(p => p.Dob),
                "dob_desc" => q.OrderByDescending(p => p.Dob),
                "name_desc" => q.OrderByDescending(p => p.FullName),
                _ => q.OrderBy(p => p.FullName)
            };

            return Ok(await q.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _db.Patients.FindAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var p = await _db.Patients
                .Include(x => x.Diagnoses)
                    .ThenInclude(d => d.Doctor)
                .FirstOrDefaultAsync(x => x.PatientId == id);

            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Patient p)
        {
            _db.Patients.Add(p);
            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Patient p)
        {
            if (id != p.PatientId) return BadRequest();
            _db.Entry(p).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Patients.FindAsync(id);
            if (p == null) return NotFound();
            _db.Patients.Remove(p);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
