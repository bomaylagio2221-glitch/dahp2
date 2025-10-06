using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.Models;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinhVucController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LinhVucController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LinhVuc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinhVuc>>> GetLinhVucs()
        {
            return await _context.LinhVucs
                .Include(l => l.NganhHocs)
                .ToListAsync();
        }

        // GET: api/LinhVuc/LV01
        [HttpGet("{id}")]
        public async Task<ActionResult<LinhVuc>> GetLinhVuc(string id)
        {
            var linhVuc = await _context.LinhVucs
                .Include(l => l.NganhHocs)
                .FirstOrDefaultAsync(l => l.MaLinhVuc == id);

            if (linhVuc == null)
                return NotFound(new { message = "Không tìm thấy lĩnh vực" });

            return linhVuc;
        }

        // POST: api/LinhVuc
        [HttpPost]
        public async Task<ActionResult<LinhVuc>> PostLinhVuc(LinhVuc linhVuc)
        {
            if (await _context.LinhVucs.AnyAsync(l => l.MaLinhVuc == linhVuc.MaLinhVuc))
                return BadRequest(new { message = "Mã lĩnh vực đã tồn tại" });

            _context.LinhVucs.Add(linhVuc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLinhVuc), new { id = linhVuc.MaLinhVuc }, linhVuc);
        }

        // PUT: api/LinhVuc/LV01
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLinhVuc(string id, LinhVuc linhVuc)
        {
            if (id != linhVuc.MaLinhVuc)
                return BadRequest(new { message = "Mã lĩnh vực không khớp" });

            _context.Entry(linhVuc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.LinhVucs.AnyAsync(l => l.MaLinhVuc == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/LinhVuc/LV01
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinhVuc(string id)
        {
            var linhVuc = await _context.LinhVucs.FindAsync(id);
            if (linhVuc == null)
                return NotFound(new { message = "Không tìm thấy lĩnh vực" });

            _context.LinhVucs.Remove(linhVuc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}