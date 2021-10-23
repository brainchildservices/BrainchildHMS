using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class CorrectedSpellingMistakeInBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            

            migrationBuilder.RenameColumn(
                name: "NoOfAChildren",
                table: "Bookings",
                newName: "NoOfChildren");

            migrationBuilder.RenameColumn(
                name: "CancelleddDate",
                table: "Bookings",
                newName: "CancelledDate");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
