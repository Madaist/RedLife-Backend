using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Added_HospitalId_Transfusions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HospitalId",
                table: "Transfusions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Transfusions_HospitalId",
                table: "Transfusions",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfusions_AbpUsers_HospitalId",
                table: "Transfusions",
                column: "HospitalId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfusions_AbpUsers_HospitalId",
                table: "Transfusions");

            migrationBuilder.DropIndex(
                name: "IX_Transfusions_HospitalId",
                table: "Transfusions");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Transfusions");
        }
    }
}
