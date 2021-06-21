using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Added_Donations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None),
                    DonorId = table.Column<long>(nullable: false),
                    CenterId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsBloodAccepted = table.Column<bool>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    BloodType = table.Column<string>(nullable: true),
                    MedicalTestsResult = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_AbpUsers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Donations_AbpUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CenterId",
                table: "Donations",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");
        }
    }
}
