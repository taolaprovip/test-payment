using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RePurpose_Models.Migrations
{
    public partial class updatetbitem3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeriodTime",
                table: "Items",
                newName: "PickupTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupTime",
                table: "Items",
                newName: "PeriodTime");
        }
    }
}
