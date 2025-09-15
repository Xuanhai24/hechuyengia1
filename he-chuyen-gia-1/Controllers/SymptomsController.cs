//// Controllers/SymptomsController.cs
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
//    public class SymptomsController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        public SymptomsController(AppDbContext db) => _db = db;

//        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Symptoms.AsNoTracking().ToListAsync());
//        [HttpPost] public async Task<IActionResult> Create(Symptom s) { _db.Add(s); await _db.SaveChangesAsync(); return Ok(s); }
//        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Symptom s) { if (id != s.SymptomId) return BadRequest(); _db.Update(s); await _db.SaveChangesAsync(); return Ok(s); }
//        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var x = await _db.Symptoms.FindAsync(id); if (x == null) return NotFound(); _db.Remove(x); await _db.SaveChangesAsync(); return Ok(); }
//    }
//}
