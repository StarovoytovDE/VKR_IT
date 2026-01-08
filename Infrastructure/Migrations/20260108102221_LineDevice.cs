using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LineDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "line_rza_device",
                columns: table => new
                {
                    line_object_id = table.Column<long>(type: "bigint", nullable: false),
                    device_object_id = table.Column<long>(type: "bigint", nullable: false),
                    line_side = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_line_rza_device", x => new { x.line_object_id, x.device_object_id });
                    table.CheckConstraint("ck_line_rza_device_line_side", "line_side IN (1,2)");
                    table.ForeignKey(
                        name: "fk_line_rza_device_device_object",
                        column: x => x.device_object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_line_rza_device_line_object",
                        column: x => x.line_object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_line_rza_device_device",
                table: "line_rza_device",
                column: "device_object_id");

            migrationBuilder.CreateIndex(
                name: "ix_line_rza_device_line",
                table: "line_rza_device",
                column: "line_object_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "line_rza_device");
        }
    }
}
