using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentJobManagement.Models
{
    [Table("SINH_VIEN")]
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string TenSV { get; set; } = null!;
        
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public string? MaNganh { get; set; }
        public int? NamTotNghiep { get; set; }
        public decimal? GPA { get; set; }
        public string? XepLoaiTN { get; set; }

        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }
        
        public virtual ICollection<ViecLamSV> ViecLams { get; set; } = new List<ViecLamSV>();
        public virtual ICollection<HocNangCao> HocNangCaos { get; set; } = new List<HocNangCao>();
    }

    [Table("NGANH_HOC")]
    public class NganhHoc
    {
        [Key]
        public string MaNganh { get; set; } = null!;
        
        [Required]
        public string TenNganh { get; set; } = null!;
        public string? MaLinhVuc { get; set; }
        public string? MoTa { get; set; }

        [ForeignKey("MaLinhVuc")]
        public virtual LinhVuc? LinhVuc { get; set; }
        
        public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }

    [Table("LINH_VUC")]
    public class LinhVuc
    {
        [Key]
        public string MaLinhVuc { get; set; } = null!;
        
        [Required]
        public string TenLinhVuc { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<NganhHoc> NganhHocs { get; set; } = new List<NganhHoc>();
    }

    [Table("VIEC_LAM_SV")]
    public class ViecLamSV
    {
        [Key]
        public int MaViecLam { get; set; }
        
        [Required]
        public string MaSV { get; set; } = null!;
        public string? TenCongTy { get; set; }
        public string? ViTri { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public decimal? MucLuong { get; set; }
        public bool DungNganh { get; set; }
        public string? TrangThai { get; set; }
        public string? HinhThuc { get; set; }
        public string? DiaDiem { get; set; }
        public string? GhiChu { get; set; }

        [ForeignKey("MaSV")]
        public virtual SinhVien? SinhVien { get; set; }
    }

    [Table("HOC_NANG_CAO")]
    public class HocNangCao
    {
        [Key]
        public int MaHNC { get; set; }
        
        [Required]
        public string MaSV { get; set; } = null!;
        public string? TruongHoc { get; set; }
        public string? ChuyenNganh { get; set; }
        public string? BacHoc { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? QuocGia { get; set; }
        public string? TrangThai { get; set; }

        [ForeignKey("MaSV")]
        public virtual SinhVien? SinhVien { get; set; }
    }
}