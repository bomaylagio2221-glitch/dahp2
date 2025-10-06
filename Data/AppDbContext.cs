using Microsoft.EntityFrameworkCore;
using StudentJobManagement.Models;

namespace StudentJobManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; } = null!;
        public DbSet<NganhHoc> NganhHocs { get; set; } = null!;
        public DbSet<LinhVuc> LinhVucs { get; set; } = null!;
        public DbSet<ViecLamSV> ViecLamSVs { get; set; } = null!;
        public DbSet<HocNangCao> HocNangCaos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ
            modelBuilder.Entity<SinhVien>()
                .HasOne(s => s.NganhHoc)
                .WithMany(n => n.SinhViens)
                .HasForeignKey(s => s.MaNganh)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NganhHoc>()
                .HasOne(n => n.LinhVuc)
                .WithMany(l => l.NganhHocs)
                .HasForeignKey(n => n.MaLinhVuc)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ViecLamSV>()
                .HasOne(v => v.SinhVien)
                .WithMany(s => s.ViecLams)
                .HasForeignKey(v => v.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HocNangCao>()
                .HasOne(h => h.SinhVien)
                .WithMany(s => s.HocNangCaos)
                .HasForeignKey(h => h.MaSV)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}