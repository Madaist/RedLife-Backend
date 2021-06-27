using Microsoft.EntityFrameworkCore.Migrations;

namespace RedLife.Migrations
{
    public partial class Added_BloodReceiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BloodReceiver",
                table: "Donations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HospitalReceiver",
                table: "Donations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodReceiver",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "HospitalReceiver",
                table: "Donations");
        }
    }
}
