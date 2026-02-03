using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Ct_places : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ct_place_devices_device_id",
                table: "ct_place");

            migrationBuilder.DropForeignKey(
                name: "fk_device_main_vt",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "fk_device_reserve_vt",
                table: "device");

            migrationBuilder.DropIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place");

            migrationBuilder.DropColumn(
                name: "device_id",
                table: "ct_place");

            migrationBuilder.AddColumn<long>(
                name: "ct_place_id",
                table: "device",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_ct_place_id",
                table: "device",
                column: "ct_place_id");

            migrationBuilder.CreateIndex(
                name: "ix_ct_place_place_code",
                table: "ct_place",
                column: "place_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_device_ct_place",
                table: "device",
                column: "ct_place_id",
                principalTable: "ct_place",
                principalColumn: "ct_place_id");

            migrationBuilder.AddForeignKey(
                name: "fk_device_main_vt",
                table: "device",
                column: "main_vt_id",
                principalTable: "vt",
                principalColumn: "vt_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_device_reserve_vt",
                table: "device",
                column: "reserve_vt_id",
                principalTable: "vt",
                principalColumn: "vt_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_device_ct_place",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "fk_device_main_vt",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "fk_device_reserve_vt",
                table: "device");

            migrationBuilder.DropIndex(
                name: "ix_device_ct_place_id",
                table: "device");

            migrationBuilder.DropIndex(
                name: "ix_ct_place_place_code",
                table: "ct_place");

            migrationBuilder.DropColumn(
                name: "ct_place_id",
                table: "device");

            migrationBuilder.AddColumn<long>(
                name: "device_id",
                table: "ct_place",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place",
                column: "device_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_ct_place_devices_device_id",
                table: "ct_place",
                column: "device_id",
                principalTable: "device",
                principalColumn: "device_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_device_main_vt",
                table: "device",
                column: "main_vt_id",
                principalTable: "vt",
                principalColumn: "vt_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_device_reserve_vt",
                table: "device",
                column: "reserve_vt_id",
                principalTable: "vt",
                principalColumn: "vt_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
