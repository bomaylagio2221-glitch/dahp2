using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.Models;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViecLamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViecLamController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ViecLam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViecLamSV>>> GetViecLams()
        {
            return await _context.ViecLamSVs
                .Include(v => v.SinhVien)
                    .ThenInclude(s => s!.NganhHoc)
                .OrderByDescending(v => v.NgayBatDau)
                .ToListAsync();
        }

        // GET: api/ViecLam/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViecLamSV>> GetViecLam(int id)
        {
            var viecLam = await _context.ViecLamSVs
                .Include(v => v.SinhVien)
                    .ThenInclude(s => s!.NganhHoc)
                .FirstOrDefaultAsync(v => v.MaViecLam == id);

            if (viecLam == null)
                return NotFound(new { message = "Không tìm thấy thông tin việc làm" });

            return viecLam;
        }

        // GET: api/ViecLam/BySinhVien/SV001
        [HttpGet("BySinhVien/{maSV}")]
        public async Task<ActionResult<IEnumerable<ViecLamSV>>> GetViecLamBySinhVien(string maSV)
        {
            var viecLams = await _context.ViecLamSVs
                .Where(v => v.MaSV == maSV)
                .OrderByDescending(v => v.NgayBatDau)
                .ToListAsync();

            return Ok(viecLams);
        }

        // POST: api/ViecLam
        [HttpPost]
        public async Task<ActionResult<ViecLamSV>> PostViecLam(ViecLamSV viecLam)
        {
            // Kiểm tra sinh viên tồn tại
            if (!await _context.SinhViens.AnyAsync(s => s.MaSV == viecLam.MaSV))
                return BadRequest(new { message = "Mã sinh viên không tồn tại" });

            _context.ViecLamSVs.Add(viecLam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetViecLam), new { id = viecLam.MaViecLam }, viecLam);
        }

        // PUT: api/ViecLam/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutViecLam(int id, ViecLamSV viecLam)
        {
            if (id != viecLam.MaViecLam)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(viecLam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ViecLamSVs.AnyAsync(v => v.MaViecLam == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/ViecLam/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteViecLam(int id)
        {
            var viecLam = await _context.ViecLamSVs.FindAsync(id);
            if (viecLam == null)
                return NotFound(new { message = "Không tìm thấy thông tin việc làm" });

            _context.ViecLamSVs.Remove(viecLam);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        // GET: api/ViecLam/ThongKe/DungNganh
        [HttpGet("ThongKe/DungNganh")]
        public async Task<ActionResult<object>> GetThongKeDungNganh()
        {
            var total = await _context.ViecLamSVs
                .Where(v => v.TrangThai == "Đang làm")
                .CountAsync();

            var dungNganh = await _context.ViecLamSVs
                .Where(v => v.TrangThai == "Đang làm" && v.DungNganh)
                .CountAsync();

            var khongDungNganh = total - dungNganh;

            return Ok(new
            {
                TongSo = total,
                DungNganh = dungNganh,
                KhongDungNganh = khongDungNganh,
                TyLeDungNganh = total > 0 ? Math.Round((double)dungNganh / total * 100, 2) : 0
            });
        }
    }
}