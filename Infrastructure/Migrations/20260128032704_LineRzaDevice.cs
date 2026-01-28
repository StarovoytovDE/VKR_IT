using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LineRzaDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_device_object_object_id",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "fk_object_substations_substation_id",
                table: "object");

            migrationBuilder.DropIndex(
                name: "ix_object_substation_id",
                table: "object");

            migrationBuilder.DropColumn(
                name: "substation_id",
                table: "object");

            migrationBuilder.RenameColumn(
                name: "object_id",
                table: "device",
                newName: "line_end_id");

            migrationBuilder.RenameIndex(
                name: "ix_device_object_id",
                table: "device",
                newName: "ix_device_line_end_id");

            migrationBuilder.CreateTable(
                name: "line_end",
                columns: table => new
                {
                    line_end_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<long>(type: "bigint", nullable: false),
                    substation_id = table.Column<long>(type: "bigint", nullable: false),
                    side_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_line_end", x => x.line_end_id);
                    table.ForeignKey(
                        name: "fk_line_end_object",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_line_end_substation",
                        column: x => x.substation_id,
                        principalTable: "substations",
                        principalColumn: "substation_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_line_end_substation_id",
                table: "line_end",
                column: "substation_id");

            migrationBuilder.CreateIndex(
                name: "uq_line_end_object_side",
                table: "line_end",
                columns: new[] { "object_id", "side_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_line_end_object_substation",
                table: "line_end",
                columns: new[] { "object_id", "substation_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_device_line_end",
                table: "device",
                column: "line_end_id",
                principalTable: "line_end",
                principalColumn: "line_end_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_device_line_end",
                table: "device");

            migrationBuilder.DropTable(
                name: "line_end");

            migrationBuilder.RenameColumn(
                name: "line_end_id",
                table: "device",
                newName: "object_id");

            migrationBuilder.RenameIndex(
                name: "ix_device_line_end_id",
                table: "device",
                newName: "ix_device_object_id");

            migrationBuilder.AddColumn<long>(
                name: "substation_id",
                table: "object",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_object_substation_id",
                table: "object",
                column: "substation_id");

            migrationBuilder.AddForeignKey(
                name: "fk_device_object_object_id",
                table: "device",
                column: "object_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_object_substations_substation_id",
                table: "object",
                column: "substation_id",
                principalTable: "substations",
                principalColumn: "substation_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
