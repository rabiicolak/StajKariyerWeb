using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StajKariyerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CompanyProfiles",
                columns: new[] { "Id", "City", "CreatedAt", "Description", "LogoPath", "Name", "RelatedArea", "RequiredSkills", "Sector", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "İstanbul", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yapay zeka ve derin öğrenme modelleri geliştiren yenilikçi teknoloji firması.", null, "DeepMindTR", "Yapay Zeka", "Python, TensorFlow, PyTorch, Makine Öğrenmesi", "Teknoloji", "https://example.com/deepmindtr" },
                    { 2, "Ankara", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Büyük veri analizi, iş zekası ve veri madenciliği çözümleri sunar.", null, "DataSense", "Veri Bilimi", "SQL, Python, Power BI, Veri Modelleme", "Veri Analitiği", "https://example.com/datasense" },
                    { 3, "İzmir", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern, hızlı ve ölçeklenebilir web uygulamaları ve e-ticaret altyapıları tasarlar.", null, "WebSoft", "Web", "HTML/CSS, JavaScript, React, ASP.NET Core", "Yazılım Geliştirme", "https://example.com/websoft" },
                    { 4, "İstanbul", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek trafikli sistemler için güvenli, performanslı ve sağlam API altyapıları oluşturur.", null, "BackendPro", "Backend", "C#, Java, SQL Server, Microservices, Docker", "Finans & Teknoloji", "https://example.com/backendpro" },
                    { 5, "Ankara", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kurumsal sızma testleri, ağ güvenliği ve güvenlik danışmanlığı hizmeti verir.", null, "CyberSec", "Siber Güvenlik", "Ağ Güvenliği, Sızma Testi, Linux, OWASP", "Siber Güvenlik", "https://example.com/cybersec" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyProfiles");
        }
    }
}
