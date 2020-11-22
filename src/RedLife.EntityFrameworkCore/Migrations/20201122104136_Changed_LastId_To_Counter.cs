using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Changed_LastId_To_Counter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastId",
                table: "LastUserId");

            migrationBuilder.AddColumn<long>(
                name: "Counter",
                table: "LastUserId",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "LastUserId");

            migrationBuilder.AddColumn<long>(
                name: "LastId",
                table: "LastUserId",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
