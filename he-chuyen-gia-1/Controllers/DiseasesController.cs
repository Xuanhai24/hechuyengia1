//// Controllers/DiseasesController.cs
//using hechuyengia.Data;
//using hechuyengia.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace hechuyengia.Controllers
//{
//    [Authorize(Roles = "DOCTOR,ADMIN")]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DiseasesController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        public DiseasesController(AppDbContext db) => _db = db;

//        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Diseases.Include(d => d.DiseaseSymptoms).ThenInclude(ds => ds.Symptom).ToListAsync());

//        public record BindDto(int DiseaseId, int SymptomId);
//        [HttpPost] public async Task<IActionResult> Create(Disease d) { _db.Add(d); await _db.SaveChangesAsync(); return Ok(d); }
//        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Disease d) { if (id != d.DiseaseId) return BadRequest(); _db.Update(d); await _db.SaveChangesAsync(); return Ok(d); }
//        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var x = await _db.Diseases.FindAsync(id); if (x == null) return NotFound(); _db.Remove(x); await _db.SaveChangesAsync(); return Ok(); }

//        // Gắn/huỷ gắn triệu chứng cho bệnh
//        [HttpPost("bind")]
//        public async Task<IActionResult> BindSymptom([FromBody] BindDto dto)
//        {
//            if (!await _db.Diseases.AnyAsync(d => d.DiseaseId == dto.DiseaseId) ||
//                !await _db.Symptoms.AnyAsync(s => s.SymptomId == dto.SymptomId))
//                return NotFound();

//            if (!await _db.DiseaseSymptoms.AnyAsync(x => x.DiseaseId == dto.DiseaseId && x.SymptomId == dto.SymptomId))
//                _db.DiseaseSymptoms.Add(new DiseaseSymptom { DiseaseId = dto.DiseaseId, SymptomId = dto.SymptomId });
//            await _db.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPost("unbind")]
//        public async Task<IActionResult> UnbindSymptom([FromBody] BindDto dto)
//        {
//            var x = await _db.DiseaseSymptoms.FirstOrDefaultAsync(y => y.DiseaseId == dto.DiseaseId && y.SymptomId == dto.SymptomId);
//            if (x == null) return NotFound();
//            _db.DiseaseSymptoms.Remove(x); await _db.SaveChangesAsync(); return Ok();
//        }
//    }
//}
