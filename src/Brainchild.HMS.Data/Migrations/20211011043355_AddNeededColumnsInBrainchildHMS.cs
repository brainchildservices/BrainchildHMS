using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class AddNeededColumnsInBrainchildHMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentAdvance",
                table: "Payments",
                newName: "PaymentDescription");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Charges",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChargeId",
                table: "Billings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Billings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Charges_RoomId",
                table: "Charges",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_ChargeId",
                table: "Billings",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_RoomId",
                table: "Billings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Charges_ChargeId",
                table: "Billings",
                column: "ChargeId",
                principalTable: "Charges",
                principalColumn: "ChargeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Rooms_RoomId",
                table: "Billings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Rooms_RoomId",
                table: "Charges",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Charges_ChargeId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Rooms_RoomId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Rooms_RoomId",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Charges_RoomId",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Billings_ChargeId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_RoomId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "ChargeId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "PaymentDescription",
                table: "Payments",
                newName: "PaymentAdvance");
        }
    }
}
