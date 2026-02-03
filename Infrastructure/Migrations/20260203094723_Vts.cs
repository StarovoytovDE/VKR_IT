using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Vts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vt_device_device_id",
                table: "vt");

            migrationBuilder.DropIndex(
                name: "ix_vt_device_id_main",
                table: "vt");

            migrationBuilder.DropColumn(
                name: "device_id",
                table: "vt");

            migrationBuilder.DropColumn(
                name: "main",
                table: "vt");

            migrationBuilder.AddColumn<long>(
                name: "main_vt_id",
                table: "device",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "reserve_vt_id",
                table: "device",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_device_main_vt_id",
                table: "device",
                column: "main_vt_id");

            migrationBuilder.CreateIndex(
                name: "ix_device_reserve_vt_id",
                table: "device",
                column: "reserve_vt_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_device_main_vt",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "fk_device_reserve_vt",
                table: "device");

            migrationBuilder.DropIndex(
                name: "ix_device_main_vt_id",
                table: "device");

            migrationBuilder.DropIndex(
                name: "ix_device_reserve_vt_id",
                table: "device");

            migrationBuilder.DropColumn(
                name: "main_vt_id",
                table: "device");

            migrationBuilder.DropColumn(
                name: "reserve_vt_id",
                table: "device");

            migrationBuilder.AddColumn<long>(
                name: "device_id",
                table: "vt",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "main",
                table: "vt",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_vt_device_id_main",
                table: "vt",
                columns: new[] { "device_id", "main" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_vt_device_device_id",
                table: "vt",
                column: "device_id",
                principalTable: "device",
                principalColumn: "device_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
