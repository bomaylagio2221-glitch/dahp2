using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentJobManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LINH_VUC",
                columns: table => new
                {
                    MaLinhVuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLinhVuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LINH_VUC", x => x.MaLinhVuc);
                });

            migrationBuilder.CreateTable(
                name: "NGANH_HOC",
                columns: table => new
                {
                    MaNganh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLinhVuc = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGANH_HOC", x => x.MaNganh);
                    table.ForeignKey(
                        name: "FK_NGANH_HOC_LINH_VUC_MaLinhVuc",
                        column: x => x.MaLinhVuc,
                        principalTable: "LINH_VUC",
                        principalColumn: "MaLinhVuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SINH_VIEN",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNganh = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NamTotNghiep = table.Column<int>(type: "int", nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    XepLoaiTN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINH_VIEN", x => x.MaSV);
                    table.ForeignKey(
                        name: "FK_SINH_VIEN_NGANH_HOC_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "NGANH_HOC",
                        principalColumn: "MaNganh",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HOC_NANG_CAO",
                columns: table => new
                {
                    MaHNC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TruongHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChuyenNganh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BacHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOC_NANG_CAO", x => x.MaHNC);
                    table.ForeignKey(
                        name: "FK_HOC_NANG_CAO_SINH_VIEN_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SINH_VIEN",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VIEC_LAM_SV",
                columns: table => new
                {
                    MaViecLam = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenCongTy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViTri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MucLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DungNganh = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIEC_LAM_SV", x => x.MaViecLam);
                    table.ForeignKey(
                        name: "FK_VIEC_LAM_SV_SINH_VIEN_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SINH_VIEN",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HOC_NANG_CAO_MaSV",
                table: "HOC_NANG_CAO",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_NGANH_HOC_MaLinhVuc",
                table: "NGANH_HOC",
                column: "MaLinhVuc");

            migrationBuilder.CreateIndex(
                name: "IX_SINH_VIEN_MaNganh",
                table: "SINH_VIEN",
                column: "MaNganh");

            migrationBuilder.CreateIndex(
                name: "IX_VIEC_LAM_SV_MaSV",
                table: "VIEC_LAM_SV",
                column: "MaSV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOC_NANG_CAO");

            migrationBuilder.DropTable(
                name: "VIEC_LAM_SV");

            migrationBuilder.DropTable(
                name: "SINH_VIEN");

            migrationBuilder.DropTable(
                name: "NGANH_HOC");

            migrationBuilder.DropTable(
                name: "LINH_VUC");
        }
    }
}
