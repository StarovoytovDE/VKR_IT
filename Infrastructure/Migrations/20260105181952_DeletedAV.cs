using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedAV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_action_param_requirement_action_version",
                table: "action_param_requirement");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_action_version",
                table: "instruction_request");

            migrationBuilder.DropTable(
                name: "action_version");

            migrationBuilder.RenameColumn(
                name: "action_version_id",
                table: "instruction_request",
                newName: "action_id");

            migrationBuilder.RenameIndex(
                name: "ix_instruction_request_action_version_id",
                table: "instruction_request",
                newName: "ix_instruction_request_action_id");

            migrationBuilder.RenameColumn(
                name: "action_version_id",
                table: "action_param_requirement",
                newName: "action_id");

            migrationBuilder.AddForeignKey(
                name: "fk_action_param_requirement_action",
                table: "action_param_requirement",
                column: "action_id",
                principalTable: "action",
                principalColumn: "action_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_action",
                table: "instruction_request",
                column: "action_id",
                principalTable: "action",
                principalColumn: "action_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_action_param_requirement_action",
                table: "action_param_requirement");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_action",
                table: "instruction_request");

            migrationBuilder.RenameColumn(
                name: "action_id",
                table: "instruction_request",
                newName: "action_version_id");

            migrationBuilder.RenameIndex(
                name: "ix_instruction_request_action_id",
                table: "instruction_request",
                newName: "ix_instruction_request_action_version_id");

            migrationBuilder.RenameColumn(
                name: "action_id",
                table: "action_param_requirement",
                newName: "action_version_id");

            migrationBuilder.CreateTable(
                name: "action_version",
                columns: table => new
                {
                    action_version_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    action_id = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    released_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    version_label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_version", x => x.action_version_id);
                    table.ForeignKey(
                        name: "fk_action_version_action",
                        column: x => x.action_id,
                        principalTable: "action",
                        principalColumn: "action_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "uq_action_version_action_active",
                table: "action_version",
                column: "action_id",
                unique: true,
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "uq_action_version_action_label",
                table: "action_version",
                columns: new[] { "action_id", "version_label" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_action_param_requirement_action_version",
                table: "action_param_requirement",
                column: "action_version_id",
                principalTable: "action_version",
                principalColumn: "action_version_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_action_version",
                table: "instruction_request",
                column: "action_version_id",
                principalTable: "action_version",
                principalColumn: "action_version_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
