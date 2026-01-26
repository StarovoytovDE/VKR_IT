using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Device_CtRemainsEnergized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "a_to_b_true",
                table: "mtz_busbar");

            migrationBuilder.AddColumn<bool>(
                name: "ct_remains_energized",
                table: "device",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ct_remains_energized",
                table: "device");

            migrationBuilder.AddColumn<bool>(
                name: "a_to_b_true",
                table: "mtz_busbar",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
