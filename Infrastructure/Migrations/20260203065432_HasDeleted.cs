using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HasDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "haz_dzl",
                table: "dzl");

            migrationBuilder.DropColumn(
                name: "haz_dz",
                table: "dz");

            migrationBuilder.DropColumn(
                name: "haz_dfz",
                table: "dfz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "haz_dzl",
                table: "dzl",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "haz_dz",
                table: "dz",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "haz_dfz",
                table: "dfz",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
