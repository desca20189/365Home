using Microsoft.EntityFrameworkCore.Migrations;

namespace _365Home.DataAccess.Migrations
{
    public partial class addReportFilterAndReportFilterValueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportFilter",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ReportId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportFilter_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportFilterValue",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ReportFilterId = table.Column<string>(nullable: false),
                    ReportId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFilterValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportFilterValue_ReportFilter_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ReportFilter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportFilter_ReportId",
                table: "ReportFilter",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportFilterValue_ReportId",
                table: "ReportFilterValue",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportFilterValue");

            migrationBuilder.DropTable(
                name: "ReportFilter");
        }
    }
}
