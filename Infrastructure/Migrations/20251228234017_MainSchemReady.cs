using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MainSchemReady : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "action",
                columns: table => new
                {
                    action_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action", x => x.action_id);
                });

            migrationBuilder.CreateTable(
                name: "app_role",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "app_user",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "object_type",
                columns: table => new
                {
                    object_type_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_object_type", x => x.object_type_id);
                });

            migrationBuilder.CreateTable(
                name: "param_domain",
                columns: table => new
                {
                    param_domain_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_param_domain", x => x.param_domain_id);
                });

            migrationBuilder.CreateTable(
                name: "action_version",
                columns: table => new
                {
                    action_version_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    action_id = table.Column<long>(type: "bigint", nullable: false),
                    version_label = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    released_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_role_role",
                        column: x => x.role_id,
                        principalTable: "app_role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role_user",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "object",
                columns: table => new
                {
                    object_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_type_id = table.Column<long>(type: "bigint", nullable: false),
                    uid = table.Column<string>(type: "text", nullable: false),
                    dispatch_name = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_object", x => x.object_id);
                    table.ForeignKey(
                        name: "fk_object_object_type",
                        column: x => x.object_type_id,
                        principalTable: "object_type",
                        principalColumn: "object_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "param_definition",
                columns: table => new
                {
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    param_domain_id = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value_type = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_param_definition", x => x.param_definition_id);
                    table.CheckConstraint("ck_param_definition_domain", "((value_type = 'enum' AND param_domain_id IS NOT NULL) OR (value_type <> 'enum' AND param_domain_id IS NULL))");
                    table.CheckConstraint("ck_param_definition_value_type", "value_type IN ('bool','enum','int','decimal','text','json')");
                    table.ForeignKey(
                        name: "fk_param_definition_domain",
                        column: x => x.param_domain_id,
                        principalTable: "param_domain",
                        principalColumn: "param_domain_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "param_domain_value",
                columns: table => new
                {
                    param_domain_id = table.Column<long>(type: "bigint", nullable: false),
                    value_code = table.Column<int>(type: "integer", nullable: false),
                    value_name = table.Column<string>(type: "text", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_param_domain_value", x => new { x.param_domain_id, x.value_code });
                    table.CheckConstraint("ck_param_domain_value_sort_order", "sort_order >= 0");
                    table.ForeignKey(
                        name: "fk_param_domain_value_domain",
                        column: x => x.param_domain_id,
                        principalTable: "param_domain",
                        principalColumn: "param_domain_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "instruction_request",
                columns: table => new
                {
                    instruction_request_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<long>(type: "bigint", nullable: false),
                    action_version_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instruction_request", x => x.instruction_request_id);
                    table.CheckConstraint("ck_instruction_request_status", "status IN ('DRAFT','CALCULATED','SAVED','ERROR')");
                    table.ForeignKey(
                        name: "fk_instruction_request_action_version",
                        column: x => x.action_version_id,
                        principalTable: "action_version",
                        principalColumn: "action_version_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_instruction_request_created_by",
                        column: x => x.created_by_user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_instruction_request_object",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "action_param_requirement",
                columns: table => new
                {
                    action_version_id = table.Column<long>(type: "bigint", nullable: false),
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    value_kind = table.Column<string>(type: "text", nullable: false),
                    ui_prompt = table.Column<string>(type: "text", nullable: true),
                    validation_rule = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_param_requirement", x => new { x.action_version_id, x.param_definition_id });
                    table.CheckConstraint("ck_action_param_requirement_sort_order", "sort_order >= 0");
                    table.CheckConstraint("ck_action_param_requirement_value_kind", "value_kind IN ('CONSTANT','VARIABLE')");
                    table.ForeignKey(
                        name: "fk_action_param_requirement_action_version",
                        column: x => x.action_version_id,
                        principalTable: "action_version",
                        principalColumn: "action_version_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_action_param_requirement_param_definition",
                        column: x => x.param_definition_id,
                        principalTable: "param_definition",
                        principalColumn: "param_definition_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "object_param_value",
                columns: table => new
                {
                    object_id = table.Column<long>(type: "bigint", nullable: false),
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    updated_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    value_bool = table.Column<bool>(type: "boolean", nullable: true),
                    value_int_enum = table.Column<int>(type: "integer", nullable: true),
                    value_decimal = table.Column<decimal>(type: "numeric", nullable: true),
                    value_text = table.Column<string>(type: "text", nullable: true),
                    value_json = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_object_param_value", x => new { x.object_id, x.param_definition_id });
                    table.CheckConstraint("ck_object_param_value_single_value", "num_nonnulls(value_bool, value_int_enum, value_decimal, value_text, value_json) <= 1");
                    table.ForeignKey(
                        name: "fk_object_param_value_object",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_object_param_value_param_definition",
                        column: x => x.param_definition_id,
                        principalTable: "param_definition",
                        principalColumn: "param_definition_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_object_param_value_updated_by",
                        column: x => x.updated_by_user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "instruction_result",
                columns: table => new
                {
                    instruction_request_id = table.Column<long>(type: "bigint", nullable: false),
                    generated_text = table.Column<string>(type: "text", nullable: false),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instruction_result", x => x.instruction_request_id);
                    table.ForeignKey(
                        name: "fk_instruction_result_request",
                        column: x => x.instruction_request_id,
                        principalTable: "instruction_request",
                        principalColumn: "instruction_request_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "request_param_value",
                columns: table => new
                {
                    instruction_request_id = table.Column<long>(type: "bigint", nullable: false),
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    value_origin = table.Column<string>(type: "text", nullable: false),
                    value_bool = table.Column<bool>(type: "boolean", nullable: true),
                    value_int_enum = table.Column<int>(type: "integer", nullable: true),
                    value_decimal = table.Column<decimal>(type: "numeric", nullable: true),
                    value_text = table.Column<string>(type: "text", nullable: true),
                    value_json = table.Column<JsonDocument>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_request_param_value", x => new { x.instruction_request_id, x.param_definition_id });
                    table.CheckConstraint("ck_request_param_value_origin", "value_origin IN ('CONSTANT_OBJECT','USER_INPUT','SCADA','CALCULATED')");
                    table.CheckConstraint("ck_request_param_value_single_value", "num_nonnulls(value_bool, value_int_enum, value_decimal, value_text, value_json) <= 1");
                    table.ForeignKey(
                        name: "fk_request_param_value_param_definition",
                        column: x => x.param_definition_id,
                        principalTable: "param_definition",
                        principalColumn: "param_definition_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_request_param_value_request",
                        column: x => x.instruction_request_id,
                        principalTable: "instruction_request",
                        principalColumn: "instruction_request_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "uq_action_code",
                table: "action",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_action_param_requirement_param_definition_id",
                table: "action_param_requirement",
                column: "param_definition_id");

            migrationBuilder.CreateIndex(
                name: "uq_action_param_requirement_sort_order",
                table: "action_param_requirement",
                columns: new[] { "action_version_id", "sort_order" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "uq_app_role_code",
                table: "app_role",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_app_user_login",
                table: "app_user",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_instruction_request_action_version_id",
                table: "instruction_request",
                column: "action_version_id");

            migrationBuilder.CreateIndex(
                name: "ix_instruction_request_created_by_user_id",
                table: "instruction_request",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_ir_error_created_at_desc",
                table: "instruction_request",
                column: "created_at",
                descending: new bool[0],
                filter: "status = 'ERROR'");

            migrationBuilder.CreateIndex(
                name: "ix_ir_object_created_at_desc",
                table: "instruction_request",
                columns: new[] { "object_id", "created_at" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "uq_object_type_dispatch_name",
                table: "object",
                columns: new[] { "object_type_id", "dispatch_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_object_uid",
                table: "object",
                column: "uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_object_param_value_param_definition_id",
                table: "object_param_value",
                column: "param_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_object_param_value_updated_by_user_id",
                table: "object_param_value",
                column: "updated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "uq_object_type_code",
                table: "object_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_param_definition_param_domain_id",
                table: "param_definition",
                column: "param_domain_id");

            migrationBuilder.CreateIndex(
                name: "uq_param_definition_code",
                table: "param_definition",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_param_domain_code",
                table: "param_domain",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_param_domain_value_name",
                table: "param_domain_value",
                columns: new[] { "param_domain_id", "value_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_param_definition_id",
                table: "request_param_value",
                column: "param_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_role_id",
                table: "user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "action_param_requirement");

            migrationBuilder.DropTable(
                name: "instruction_result");

            migrationBuilder.DropTable(
                name: "object_param_value");

            migrationBuilder.DropTable(
                name: "param_domain_value");

            migrationBuilder.DropTable(
                name: "request_param_value");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "param_definition");

            migrationBuilder.DropTable(
                name: "instruction_request");

            migrationBuilder.DropTable(
                name: "app_role");

            migrationBuilder.DropTable(
                name: "param_domain");

            migrationBuilder.DropTable(
                name: "action_version");

            migrationBuilder.DropTable(
                name: "app_user");

            migrationBuilder.DropTable(
                name: "object");

            migrationBuilder.DropTable(
                name: "action");

            migrationBuilder.DropTable(
                name: "object_type");
        }
    }
}
