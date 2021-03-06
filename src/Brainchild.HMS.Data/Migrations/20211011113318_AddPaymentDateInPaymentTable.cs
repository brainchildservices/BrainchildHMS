using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class AddPaymentDateInPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "PaymentAdvance",
            table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentDescription",
                table: "Payments",
                nullable: true);

          

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
