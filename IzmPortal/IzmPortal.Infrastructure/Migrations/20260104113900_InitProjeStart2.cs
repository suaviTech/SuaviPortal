using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzmPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitProjeStart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubMenuId",
                table: "MenuDocuments",
                newName: "MenuId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MenuDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDocuments_MenuId",
                table: "MenuDocuments",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuDocuments_Menus_MenuId",
                table: "MenuDocuments",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuDocuments_Menus_MenuId",
                table: "MenuDocuments");

            migrationBuilder.DropIndex(
                name: "IX_MenuDocuments_MenuId",
                table: "MenuDocuments");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "MenuDocuments",
                newName: "SubMenuId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MenuDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
