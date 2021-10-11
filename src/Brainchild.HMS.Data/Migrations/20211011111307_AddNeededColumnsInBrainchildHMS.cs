using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class AddNeededColumnsInBrainchildHMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Charges",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Rooms_RoomId",
                table: "Charges",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddColumn<int>(
               name: "RoomId",
               table: "Billings",
               type: "int",
               nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Rooms_RoomId",
                table: "Billings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddColumn<int>(
               name: "ChargeId",
               table: "Billings",
               type: "int",
               nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Charges_ChargeId",
                table: "Billings",
                column: "ChargeId",
                principalTable: "Charges",
                principalColumn: "ChargeId",
                onDelete: ReferentialAction.Restrict);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
