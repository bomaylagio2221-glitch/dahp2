using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.Models;
using StudentJobManagement.DTOs;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhVienController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SinhVienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SinhVien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SinhVien>>> GetSinhViens()
        {
            return await _context.SinhViens
                .Include(s => s.NganhHoc)
                    .ThenInclude(n => n != null ? n.LinhVuc : null)
                .ToListAsync();
        }

        // GET: api/SinhVien/SV001
        [HttpGet("{id}")]
        public async Task<ActionResult<SinhVienDetailDTO>> GetSinhVien(string id)
        {
            var sinhVien = await _context.SinhViens
                .Include(s => s.NganhHoc)
                    .ThenInclude(n => n!.LinhVuc)
                .Include(s => s.ViecLams)
                .Include(s => s.HocNangCaos)
                .FirstOrDefaultAsync(s => s.MaSV == id);

            if (sinhVien == null)
                return NotFound(new { message = "Không tìm thấy sinh viên" });

            var result = new SinhVienDetailDTO
            {
                MaSV = sinhVien.MaSV,
                TenSV = sinhVien.TenSV,
                Email = sinhVien.Email,
                SDT = sinhVien.SDT,
                TenNganh = sinhVien.NganhHoc?.TenNganh,
                TenLinhVuc = sinhVien.NganhHoc?.LinhVuc?.TenLinhVuc,
                NamTotNghiep = sinhVien.NamTotNghiep,
                ViecLams = sinhVien.ViecLams.ToList(),
                HocNangCaos = sinhVien.HocNangCaos.ToList()
            };

            return Ok(result);
        }

        // POST: api/SinhVien
        [HttpPost]
        public async Task<ActionResult<SinhVien>> PostSinhVien(SinhVien sinhVien)
        {
            if (await _context.SinhViens.AnyAsync(s => s.MaSV == sinhVien.MaSV))
                return BadRequest(new { message = "Mã sinh viên đã tồn tại" });

            _context.SinhViens.Add(sinhVien);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSinhVien), new { id = sinhVien.MaSV }, sinhVien);
        }

        // PUT: api/SinhVien/SV001
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSinhVien(string id, SinhVien sinhVien)
        {
            if (id != sinhVien.MaSV)
                return BadRequest(new { message = "Mã sinh viên không khớp" });

            _context.Entry(sinhVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.SinhViens.AnyAsync(s => s.MaSV == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/SinhVien/SV001
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSinhVien(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
                return NotFound(new { message = "Không tìm thấy sinh viên" });

            _context.SinhViens.Remove(sinhVien);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        // GET: api/SinhVien/Search?keyword=Nguyen
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SinhVien>>> SearchSinhVien(
            [FromQuery] string? keyword,
            [FromQuery] string? maNganh,
            [FromQuery] string? maLinhVuc)
        {
            var query = _context.SinhViens
                .Include(s => s.NganhHoc)
                    .ThenInclude(n => n!.LinhVuc)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => 
                    s.MaSV.Contains(keyword) || 
                    s.TenSV.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(maNganh))
            {
                query = query.Where(s => s.MaNganh == maNganh);
            }

            if (!string.IsNullOrEmpty(maLinhVuc))
            {
                query = query.Where(s => s.NganhHoc!.MaLinhVuc == maLinhVuc);
            }

            return await query.ToListAsync();
        }
    }
}