using hechuyengia.Data;
using hechuyengia.Models.DiseaseDiagnose;
using hechuyengia.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hechuyengia.Controllers
{
    [Route("api/chuan-doan-benh")]
    [ApiController]
    public class DiseaseDiagnoseController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly ILogger<DiseaseDiagnoseController> _logger;
        private readonly PrologService _prolog;
        private readonly AppDbContext _db;

        public DiseaseDiagnoseController(AppDbContext db, ILogger<DiseaseDiagnoseController> logger, PrologService prolog)
        {
            _db = db;
            _logger = logger;
            _prolog = prolog;
        }

        // GET danh-sach-trieu-chung
        [HttpGet("danh-sach-trieu-chung")]
        public async Task<ActionResult<List<string>>> GetSymptomList()
        {
            lock (_lock)
            {
                var res = _prolog.LayDanhSachTrieuChungAsync().Result; // hoặc await nếu muốn async thật
                if (res == null) return NotFound();
                return Ok(res);
            }
        }

        // POST chuan-doan
        [HttpPost("chuan-doan")]
        public async Task<ActionResult<List<DiagnoseResult>>> GetDisease([FromBody] string[] symptoms)
        {
            if (symptoms == null || symptoms.Length == 0)
            {
                return BadRequest("Danh sách triệu chứng không được để trống.");
            }

            lock (_lock)
            {
                var result = _prolog.ChuanDoanBenhAsync(new List<string>(symptoms)).Result; // hoặc await
                if (result == null) return NotFound();
                return Ok(result);
            }
        }

        [HttpPost("luu-ket-qua")]
        public IActionResult SaveDiagnosedResult([FromBody] DiagnoseResult ds)
        {
            var doctorName = User.Identity?.Name ?? "Unknown"; // lấy từ JWT claims
            ds.DoctorName = doctorName;
            ds.DiagnoseDate = DateTime.Now;

            _db.DiagnoseResults.Add(ds);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpGet("benh-nhan/{patientId}")]
        public async Task<ActionResult<IEnumerable<DiagnoseResult>>> GetByPatient(int patientId)
        {
            var doctorName = User.Identity?.Name ?? "Unknown";

            var results = await _db.DiagnoseResults
                .Where(d => d.PatientId == patientId && d.DoctorName == doctorName)
                .OrderByDescending(d => d.DiagnoseDate)
                .ToListAsync();

            return Ok(results);
        }


    }
}