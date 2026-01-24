using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlaceCodeAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vt_device_id",
                table: "vt");

            migrationBuilder.DropIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place");

            migrationBuilder.AddColumn<string>(
                name: "place_code",
                table: "vt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "place_code",
                table: "ct_place",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_vt_device_id_main",
                table: "vt",
                columns: new[] { "device_id", "main" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place",
                column: "device_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vt_device_id_main",
                table: "vt");

            migrationBuilder.DropIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place");

            migrationBuilder.DropColumn(
                name: "place_code",
                table: "vt");

            migrationBuilder.DropColumn(
                name: "place_code",
                table: "ct_place");

            migrationBuilder.CreateIndex(
                name: "ix_vt_device_id",
                table: "vt",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place",
                column: "device_id");
        }
    }
}
