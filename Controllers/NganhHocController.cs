using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.Models;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NganhHocController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NganhHocController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NganhHoc>>> GetNganhHocs()
        {
            return await _context.NganhHocs
                .Include(n => n.LinhVuc)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NganhHoc>> GetNganhHoc(string id)
        {
            var nganhHoc = await _context.NganhHocs
                .Include(n => n.LinhVuc)
                .FirstOrDefaultAsync(n => n.MaNganh == id);

            if (nganhHoc == null)
                return NotFound(new { message = "Không tìm thấy ngành học" });

            return nganhHoc;
        }

        [HttpPost]
        public async Task<ActionResult<NganhHoc>> PostNganhHoc(NganhHoc nganhHoc)
        {
            if (await _context.NganhHocs.AnyAsync(n => n.MaNganh == nganhHoc.MaNganh))
                return BadRequest(new { message = "Mã ngành đã tồn tại" });

            _context.NganhHocs.Add(nganhHoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNganhHoc), new { id = nganhHoc.MaNganh }, nganhHoc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNganhHoc(string id, NganhHoc nganhHoc)
        {
            if (id != nganhHoc.MaNganh)
                return BadRequest(new { message = "Mã ngành không khớp" });

            _context.Entry(nganhHoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.NganhHocs.AnyAsync(n => n.MaNganh == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNganhHoc(string id)
        {
            var nganhHoc = await _context.NganhHocs.FindAsync(id);
            if (nganhHoc == null)
                return NotFound(new { message = "Không tìm thấy ngành học" });

            _context.NganhHocs.Remove(nganhHoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<NganhHoc>>> SearchNganhHoc([FromQuery] string? keyword)
        {
            var query = _context.NganhHocs
                .Include(n => n.LinhVuc)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(n => 
                    n.MaNganh.Contains(keyword) || 
                    n.TenNganh.Contains(keyword));
            }

            return await query.ToListAsync();
        }
    }
}