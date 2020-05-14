using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class removeImageIdFromLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_WebImages_ImageId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_ImageId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Location_ImageId",
                table: "Location",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_WebImages_ImageId",
                table: "Location",
                column: "ImageId",
                principalTable: "WebImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
