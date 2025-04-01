using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReservationNumber",
                schema: "CarRental",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationNumber",
                schema: "CarRental",
                table: "Reservations");
        }
    }
}
