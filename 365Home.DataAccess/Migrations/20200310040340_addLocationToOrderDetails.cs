using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class addLocationToOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_LocationId",
                table: "OrderDetails",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Location_LocationId",
                table: "OrderDetails",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Location_LocationId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_LocationId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "OrderDetails");
        }
    }
}
