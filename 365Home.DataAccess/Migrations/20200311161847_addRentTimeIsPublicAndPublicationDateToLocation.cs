using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class addRentTimeIsPublicAndPublicationDateToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Location",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocationStatus",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeFrom",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RentTimeTo",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "LocationStatus",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RentTimeFrom",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RentTimeTo",
                table: "Location");
        }
    }
}
