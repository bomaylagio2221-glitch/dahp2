using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data;
using StudentJobManagement.DTOs;

namespace StudentJobManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThongKeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ThongKe/TongQuan
        [HttpGet("TongQuan")]
        public async Task<ActionResult<ThongKeDTO>> GetThongKeTongQuan()
        {
            var tongSV = await _context.SinhViens.CountAsync();
            var svCoViecLam = await _context.SinhViens
                .Where(s => s.ViecLams.Any(v => v.TrangThai == "Đang làm"))
                .CountAsync();
            var svDungNganh = await _context.SinhViens
                .Where(s => s.ViecLams.Any(v => v.DungNganh && v.TrangThai == "Đang làm"))
                .CountAsync();
            var svKhongDungNganh = await _context.SinhViens
                .Where(s => s.ViecLams.Any(v => !v.DungNganh && v.TrangThai == "Đang làm"))
                .CountAsync();
            var svHocNangCao = await _context.SinhViens
                .Where(s => s.HocNangCaos.Any())
                .CountAsync();
            var luongTB = await _context.ViecLamSVs
                .Where(v => v.TrangThai == "Đang làm" && v.MucLuong.HasValue)
                .AverageAsync(v => (decimal?)v.MucLuong);

            return Ok(new ThongKeDTO
            {
                TongSinhVien = tongSV,
                SoSVCoViecLam = svCoViecLam,
                SoSVDungNganh = svDungNganh,
                SoSVKhongDungNganh = svKhongDungNganh,
                SoSVHocNangCao = svHocNangCao,
                MucLuongTrungBinh = luongTB
            });
        }

        // GET: api/ThongKe/TheoNganh?maNganh=CNTT
        [HttpGet("TheoNganh")]
        public async Task<ActionResult<IEnumerable<ThongKeTheoNganhDTO>>> GetThongKeTheoNganh(
            [FromQuery] string? maNganh)
        {
            var query = _context.NganhHocs.AsQueryable();

            if (!string.IsNullOrEmpty(maNganh))
                query = query.Where(n => n.MaNganh == maNganh);

            var result = await query
                .Select(n => new ThongKeTheoNganhDTO
                {
                    MaNganh = n.MaNganh,
                    TenNganh = n.TenNganh,
                    TongSinhVien = n.SinhViens.Count,
                    SoSVCoViecLam = n.SinhViens.Count(s => s.ViecLams.Any(v => v.TrangThai == "Đang làm")),
                    SoSVDungNganh = n.SinhViens.Count(s => s.ViecLams.Any(v => v.DungNganh && v.TrangThai == "Đang làm")),
                    SoSVKhongDungNganh = n.SinhViens.Count(s => s.ViecLams.Any(v => !v.DungNganh && v.TrangThai == "Đang làm")),
                    SoSVHocNangCao = n.SinhViens.Count(s => s.HocNangCaos.Any()),
                    MucLuongTrungBinh = n.SinhViens
                        .SelectMany(s => s.ViecLams)
                        .Where(v => v.TrangThai == "Đang làm" && v.MucLuong.HasValue)
                        .Average(v => (decimal?)v.MucLuong)
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}