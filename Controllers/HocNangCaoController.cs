using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.Models;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HocNangCaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HocNangCaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HocNangCao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HocNangCao>>> GetHocNangCaos()
        {
            return await _context.HocNangCaos
                .Include(h => h.SinhVien)
                    .ThenInclude(s => s!.NganhHoc)
                .OrderByDescending(h => h.NgayBatDau)
                .ToListAsync();
        }

        // GET: api/HocNangCao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HocNangCao>> GetHocNangCao(int id)
        {
            var hocNangCao = await _context.HocNangCaos
                .Include(h => h.SinhVien)
                    .ThenInclude(s => s!.NganhHoc)
                .FirstOrDefaultAsync(h => h.MaHNC == id);

            if (hocNangCao == null)
                return NotFound(new { message = "Không tìm thấy thông tin học nâng cao" });

            return hocNangCao;
        }

        // GET: api/HocNangCao/BySinhVien/SV001
        [HttpGet("BySinhVien/{maSV}")]
        public async Task<ActionResult<IEnumerable<HocNangCao>>> GetHocNangCaoBySinhVien(string maSV)
        {
            var hocNangCaos = await _context.HocNangCaos
                .Where(h => h.MaSV == maSV)
                .OrderByDescending(h => h.NgayBatDau)
                .ToListAsync();

            return Ok(hocNangCaos);
        }

        // POST: api/HocNangCao
        [HttpPost]
        public async Task<ActionResult<HocNangCao>> PostHocNangCao(HocNangCao hocNangCao)
        {
            // Kiểm tra sinh viên tồn tại
            if (!await _context.SinhViens.AnyAsync(s => s.MaSV == hocNangCao.MaSV))
                return BadRequest(new { message = "Mã sinh viên không tồn tại" });

            _context.HocNangCaos.Add(hocNangCao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHocNangCao), new { id = hocNangCao.MaHNC }, hocNangCao);
        }

        // PUT: api/HocNangCao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHocNangCao(int id, HocNangCao hocNangCao)
        {
            if (id != hocNangCao.MaHNC)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(hocNangCao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.HocNangCaos.AnyAsync(h => h.MaHNC == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/HocNangCao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHocNangCao(int id)
        {
            var hocNangCao = await _context.HocNangCaos.FindAsync(id);
            if (hocNangCao == null)
                return NotFound(new { message = "Không tìm thấy thông tin học nâng cao" });

            _context.HocNangCaos.Remove(hocNangCao);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        // GET: api/HocNangCao/ThongKe/TheoBacHoc
        [HttpGet("ThongKe/TheoBacHoc")]
        public async Task<ActionResult<object>> GetThongKeTheoBacHoc()
        {
            var thongKe = await _context.HocNangCaos
                .GroupBy(h => h.BacHoc)
                .Select(g => new
                {
                    BacHoc = g.Key,
                    SoLuong = g.Count(),
                    DangHoc = g.Count(h => h.TrangThai == "Đang học"),
                    DaTotNghiep = g.Count(h => h.TrangThai == "Đã tốt nghiệp")
                })
                .ToListAsync();

            return Ok(thongKe);
        }
    }
}