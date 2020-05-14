using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class addLocationTypeToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "LocationTypeId",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationTypeId",
                table: "Location",
                column: "LocationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location",
                column: "LocationTypeId",
                principalTable: "LocationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationType_LocationTypeId",
                table: "Location");

            migrationBuilder.DropTable(
                name: "LocationType");

            migrationBuilder.DropIndex(
                name: "IX_Location_LocationTypeId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationTypeId",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
