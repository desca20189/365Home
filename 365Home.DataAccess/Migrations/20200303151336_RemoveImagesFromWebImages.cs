using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class RemoveImagesFromWebImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebImages_WebImages_WebImagesId",
                table: "WebImages");

            migrationBuilder.DropIndex(
                name: "IX_WebImages_WebImagesId",
                table: "WebImages");

            migrationBuilder.DropColumn(
                name: "WebImagesId",
                table: "WebImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WebImagesId",
                table: "WebImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebImages_WebImagesId",
                table: "WebImages",
                column: "WebImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebImages_WebImages_WebImagesId",
                table: "WebImages",
                column: "WebImagesId",
                principalTable: "WebImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
