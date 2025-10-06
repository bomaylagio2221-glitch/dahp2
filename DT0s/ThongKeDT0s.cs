using StudentJobManagement.Models;
namespace StudentJobManagement.DTOs
{
    public class ThongKeDTO
    {
        public int TongSinhVien { get; set; }
        public int SoSVCoViecLam { get; set; }
        public int SoSVDungNganh { get; set; }
        public int SoSVKhongDungNganh { get; set; }
        public int SoSVHocNangCao { get; set; }
        public decimal? MucLuongTrungBinh { get; set; }
    }

    public class ThongKeTheoNganhDTO
    {
        public string? MaNganh { get; set; }
        public string? TenNganh { get; set; }
        public int TongSinhVien { get; set; }
        public int SoSVCoViecLam { get; set; }
        public int SoSVDungNganh { get; set; }
        public int SoSVKhongDungNganh { get; set; }
        public int SoSVHocNangCao { get; set; }
        public decimal? MucLuongTrungBinh { get; set; }
    }

    public class SinhVienDetailDTO
    {
        public string? MaSV { get; set; }
        public string? TenSV { get; set; }
        public string? Email { get; set; }
        public string? SDT { get; set; }
        public string? TenNganh { get; set; }
        public string? TenLinhVuc { get; set; }
        public int? NamTotNghiep { get; set; }
        public List<ViecLamSV>? ViecLams { get; set; }
        public List<HocNangCao>? HocNangCaos { get; set; }
    }
    
}