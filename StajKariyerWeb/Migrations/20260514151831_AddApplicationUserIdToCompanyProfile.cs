using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajKariyerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserIdToCompanyProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CompanyProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CompanyProfiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CompanyProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CompanyProfiles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CompanyProfiles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "CompanyProfiles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_ApplicationUserId",
                table: "CompanyProfiles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfiles_AspNetUsers_ApplicationUserId",
                table: "CompanyProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfiles_AspNetUsers_ApplicationUserId",
                table: "CompanyProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_ApplicationUserId",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CompanyProfiles");
        }
    }
}
