using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzmPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialAdminWebFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "ApplicationShortcuts");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MenuDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MenuDocuments");

            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "Announcements");

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "ApplicationShortcuts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
