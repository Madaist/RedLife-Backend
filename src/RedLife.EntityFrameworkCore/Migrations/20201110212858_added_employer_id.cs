using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class added_employer_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployerId",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_EmployerId",
                table: "AbpUsers",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_AbpUsers_EmployerId",
                table: "AbpUsers",
                column: "EmployerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_AbpUsers_EmployerId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_EmployerId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "AbpUsers");
        }
    }
}
