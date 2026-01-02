using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzmPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Error2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordChange",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TcNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcePasswordChange",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TcNumber",
                table: "AspNetUsers");
        }
    }
}
