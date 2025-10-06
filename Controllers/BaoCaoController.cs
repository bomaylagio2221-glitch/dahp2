using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Data; // Thay bằng namespace của bạn
using StudentJobManagement.Models;
using System.IO;

[Route("api/[controller]")]
[ApiController]
public class BaoCaoController : ControllerBase
{
    private readonly AppDbContext _context;

    public BaoCaoController(AppDbContext context)
    {
        _context = context;
    }

    // Endpoint chính để xử lý việc xuất báo cáo
    // GET: api/BaoCao/xuat-excel?reportType=sinhvien&value=SV001
    [HttpGet("xuat-excel")]
    public async Task<IActionResult> ExportExcel(
        [FromQuery] string reportType, // Loại báo cáo: 'sinhvien', 'nganh', 'linhvuc'
        [FromQuery] string value)      // Giá trị tìm kiếm: mã SV, mã ngành, mã lĩnh vực
    {
        if (string.IsNullOrEmpty(reportType) || string.IsNullOrEmpty(value))
        {
            return BadRequest("Vui lòng cung cấp đủ loại báo cáo và giá trị.");
        }

        var studentsQuery = _context.SinhViens
                                    .Include(s => s.NganhHoc)
                                    .ThenInclude(n => n.LinhVuc)
                                    .Include(s => s.ViecLams) // Lấy thông tin việc làm
                                    .AsQueryable();

        // Xây dựng truy vấn động dựa vào loại báo cáo
        switch (reportType.ToLower())
        {
            case "sinhvien":
                studentsQuery = studentsQuery.Where(s => s.MaSV == value);
                break;
            case "nganh":
                studentsQuery = studentsQuery.Where(s => s.MaNganh == value);
                break;
            case "linhvuc":
                studentsQuery = studentsQuery.Where(s => s.NganhHoc.MaLinhVuc == value);
                break;
            default:
                return BadRequest("Loại báo cáo không hợp lệ.");
        }

        var students = await studentsQuery.ToListAsync();

        if (!students.Any())
        {
            return NotFound("Không tìm thấy dữ liệu phù hợp để xuất báo cáo.");
        }

        // Gọi hàm tạo file Excel
        var content = CreateExcelFile(students);
        string fileName = $"BaoCao_{reportType}_{value}_{DateTime.Now:yyyyMMdd}.xlsx";
        
        return File(
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }

    // Hàm private để tạo file Excel từ danh sách sinh viên
    private byte[] CreateExcelFile(List<SinhVien> students)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("BaoCaoSinhVien");
            var currentRow = 1;

            // --- Tạo Header ---
            worksheet.Cell(currentRow, 1).Value = "Mã SV";
            worksheet.Cell(currentRow, 2).Value = "Tên SV";
            worksheet.Cell(currentRow, 3).Value = "Email";
            worksheet.Cell(currentRow, 4).Value = "Ngành Học";
            worksheet.Cell(currentRow, 5).Value = "Năm Tốt Nghiệp";
            worksheet.Cell(currentRow, 6).Value = "Tên Công Ty";
            worksheet.Cell(currentRow, 7).Value = "Vị Trí";
            worksheet.Cell(currentRow, 8).Value = "Đúng Ngành";
            
            // In đậm header
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

            // --- Thêm Dữ liệu ---
            foreach (var sv in students)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = sv.MaSV;
                worksheet.Cell(currentRow, 2).Value = sv.TenSV;
                worksheet.Cell(currentRow, 3).Value = sv.Email;
                worksheet.Cell(currentRow, 4).Value = sv.NganhHoc?.TenNganh;
                worksheet.Cell(currentRow, 5).Value = sv.NamTotNghiep;

                // Lấy thông tin việc làm gần nhất (nếu có)
                var latestJob = sv.ViecLams?.OrderByDescending(j => j.NgayBatDau).FirstOrDefault();
                if (latestJob != null)
                {
                    worksheet.Cell(currentRow, 6).Value = latestJob.TenCongTy;
                    worksheet.Cell(currentRow, 7).Value = latestJob.ViTri;
                    worksheet.Cell(currentRow, 8).Value = latestJob.DungNganh ? "Có" : "Không";
                }
            }
            
            worksheet.Columns().AdjustToContents(); // Tự động điều chỉnh độ rộng cột

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}