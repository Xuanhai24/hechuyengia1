//// Controllers/RulesController.cs  (CRUD luật Prolog)
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
//    public class RulesController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        public RulesController(AppDbContext db) => _db = db;

//        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.PrologRules.AsNoTracking().ToListAsync());
//        [HttpPost] public async Task<IActionResult> Create(Rule r) { _db.Add(r); await _db.SaveChangesAsync(); return Ok(r); }
//        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Rule r) { if (id != r.RuleId) return BadRequest(); _db.Update(r); await _db.SaveChangesAsync(); return Ok(r); }
//        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var x = await _db.PrologRules.FindAsync(id); if (x == null) return NotFound(); _db.Remove(x); await _db.SaveChangesAsync(); return Ok(); }
//    }
//}
