using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Added_Transfusion_Quantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "Transfusions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Transfusions");
        }
    }
}
