using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class renameRentTimeColumnsForOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentTimeFrom",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RentTimeTo",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RentTimeFrom",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RentTimeTo",
                table: "Location");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeEndDate",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeStartDate",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeEndDate",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeStartDate",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentTimeEndDate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RentTimeStartDate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RentTimeEndDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RentTimeStartDate",
                table: "Location");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeFrom",
                table: "OrderDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeTo",
                table: "OrderDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeFrom",
                table: "Location",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeTo",
                table: "Location",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
