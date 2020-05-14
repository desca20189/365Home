using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class AddImageListToLocationAndLocationToWebImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "WebImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebImages_LocationId",
                table: "WebImages",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebImages_Location_LocationId",
                table: "WebImages",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebImages_Location_LocationId",
                table: "WebImages");

            migrationBuilder.DropIndex(
                name: "IX_WebImages_LocationId",
                table: "WebImages");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "WebImages");
        }
    }
}
