using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class AddRoomRateInRoomBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RoomRate",
                table: "RoomBookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomRate",
                table: "RoomBookings");           
        }
    }
}
