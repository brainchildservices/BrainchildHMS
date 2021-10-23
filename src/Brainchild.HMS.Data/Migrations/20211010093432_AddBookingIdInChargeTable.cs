using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class AddBookingIdInChargeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Charges",
                type: "int",
                nullable: true);          

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Bookings_BookingId",
                table: "Charges",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
