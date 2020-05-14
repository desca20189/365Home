using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class addPriceToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "LocationTypeId",
                table: "Location",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Location",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location",
                column: "LocationTypeId",
                principalTable: "LocationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "LocationTypeId",
                table: "Location",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location",
                column: "LocationTypeId",
                principalTable: "LocationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
