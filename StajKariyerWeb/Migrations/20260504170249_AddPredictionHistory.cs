using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajKariyerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddPredictionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredictionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TahminTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IlgiliAlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Proje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prediction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchedArea = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredictionHistories");
        }
    }
}
