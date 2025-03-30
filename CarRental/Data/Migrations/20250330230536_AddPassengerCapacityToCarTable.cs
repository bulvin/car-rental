using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPassengerCapacityToCarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Model",
                schema: "CarRental",
                table: "Cars",
                newName: "Model_Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "DailyRate",
                schema: "CarRental",
                table: "Cars",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "Model_PassengerCapacity",
                schema: "CarRental",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model_PassengerCapacity",
                schema: "CarRental",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "Model_Name",
                schema: "CarRental",
                table: "Cars",
                newName: "Model");

            migrationBuilder.AlterColumn<decimal>(
                name: "DailyRate",
                schema: "CarRental",
                table: "Cars",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");
        }
    }
}
