using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MtzBusbar_AToBTrue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ct_place",
                table: "mtz_busbar");

            migrationBuilder.AddColumn<bool>(
                name: "a_to_b_true",
                table: "mtz_busbar",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "a_to_b_true",
                table: "mtz_busbar");

            migrationBuilder.AddColumn<string>(
                name: "ct_place",
                table: "mtz_busbar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
