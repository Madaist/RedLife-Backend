using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Added_Transfusions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfusions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DonationId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfusions_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfusions_DonationId",
                table: "Transfusions",
                column: "DonationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfusions");
        }
    }
}
