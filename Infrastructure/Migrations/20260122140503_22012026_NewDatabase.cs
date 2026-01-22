using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _22012026_NewDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_action",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_created_by",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_object",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_result_request",
                table: "instruction_result");

            migrationBuilder.DropForeignKey(
                name: "fk_object_object_type",
                table: "object");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_param_definition",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_request",
                table: "request_param_value");

            migrationBuilder.DropTable(
                name: "action_param_requirement");

            migrationBuilder.DropTable(
                name: "line_rza_device");

            migrationBuilder.DropTable(
                name: "object_param_value");

            migrationBuilder.DropTable(
                name: "param_domain_value");

            migrationBuilder.DropTable(
                name: "param_definition");

            migrationBuilder.DropTable(
                name: "param_domain");

            migrationBuilder.DropPrimaryKey(
                name: "pk_request_param_value",
                table: "request_param_value");

            migrationBuilder.DropCheckConstraint(
                name: "ck_request_param_value_origin",
                table: "request_param_value");

            migrationBuilder.DropCheckConstraint(
                name: "ck_request_param_value_single_value",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "uq_object_type_dispatch_name",
                table: "object");

            migrationBuilder.DropIndex(
                name: "uq_object_uid",
                table: "object");

            migrationBuilder.DropIndex(
                name: "ix_ir_error_created_at_desc",
                table: "instruction_request");

            migrationBuilder.DropIndex(
                name: "ix_ir_object_created_at_desc",
                table: "instruction_request");

            migrationBuilder.DropCheckConstraint(
                name: "ck_instruction_request_status",
                table: "instruction_request");

            migrationBuilder.DropIndex(
                name: "uq_app_user_login",
                table: "app_user");

            migrationBuilder.DropIndex(
                name: "uq_action_code",
                table: "action");

            migrationBuilder.DropColumn(
                name: "value_bool",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "value_decimal",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "value_int_enum",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "value_json",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "value_origin",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "value_text",
                table: "request_param_value");

            migrationBuilder.RenameColumn(
                name: "param_definition_id",
                table: "request_param_value",
                newName: "device_id");

            migrationBuilder.RenameIndex(
                name: "ix_request_param_value_param_definition_id",
                table: "request_param_value",
                newName: "ix_request_param_value_device_id");

            migrationBuilder.AddColumn<long>(
                name: "request_param_value_id",
                table: "request_param_value",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "dfz_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "dz_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "dzl_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "mtz_busbar_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "oapv_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "tapv_id",
                table: "request_param_value",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "object",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<long>(
                name: "substation_id",
                table: "object",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "instruction_request",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "pk_request_param_value",
                table: "request_param_value",
                column: "request_param_value_id");

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    device_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    vt_switch_true = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device", x => x.device_id);
                    table.ForeignKey(
                        name: "fk_device_object_object_id",
                        column: x => x.object_id,
                        principalTable: "object",
                        principalColumn: "object_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "substations",
                columns: table => new
                {
                    substation_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dispatch_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_substations", x => x.substation_id);
                });

            migrationBuilder.CreateTable(
                name: "ct_place",
                columns: table => new
                {
                    ct_place_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    place = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ct_place", x => x.ct_place_id);
                    table.ForeignKey(
                        name: "fk_ct_place_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dfz",
                columns: table => new
                {
                    dfz_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    haz_dfz = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dfz", x => x.dfz_id);
                    table.ForeignKey(
                        name: "fk_dfz_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dz",
                columns: table => new
                {
                    dz_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    haz_dz = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dz", x => x.dz_id);
                    table.ForeignKey(
                        name: "fk_dz_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dzl",
                columns: table => new
                {
                    dzl_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    haz_dzl = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dzl", x => x.dzl_id);
                    table.ForeignKey(
                        name: "fk_dzl_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mtz_busbar",
                columns: table => new
                {
                    mtz_busbar_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    ct_place = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mtz_busbar", x => x.mtz_busbar_id);
                    table.ForeignKey(
                        name: "fk_mtz_busbar_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oapv",
                columns: table => new
                {
                    oapv_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    switch_off = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_oapv", x => x.oapv_id);
                    table.ForeignKey(
                        name: "fk_oapv_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tapv",
                columns: table => new
                {
                    tapv_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    switch_off = table.Column<bool>(type: "boolean", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tapv", x => x.tapv_id);
                    table.ForeignKey(
                        name: "fk_tapv_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vt",
                columns: table => new
                {
                    vt_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<long>(type: "bigint", nullable: false),
                    main = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    place = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vt", x => x.vt_id);
                    table.ForeignKey(
                        name: "fk_vt_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "device_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_dfz_id",
                table: "request_param_value",
                column: "dfz_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_dz_id",
                table: "request_param_value",
                column: "dz_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_dzl_id",
                table: "request_param_value",
                column: "dzl_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_instruction_request_id",
                table: "request_param_value",
                column: "instruction_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_mtz_busbar_id",
                table: "request_param_value",
                column: "mtz_busbar_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_oapv_id",
                table: "request_param_value",
                column: "oapv_id");

            migrationBuilder.CreateIndex(
                name: "ix_request_param_value_tapv_id",
                table: "request_param_value",
                column: "tapv_id");

            migrationBuilder.CreateIndex(
                name: "ix_object_object_type_id",
                table: "object",
                column: "object_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_object_substation_id",
                table: "object",
                column: "substation_id");

            migrationBuilder.CreateIndex(
                name: "ix_instruction_request_object_id",
                table: "instruction_request",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_ct_place_device_id",
                table: "ct_place",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_device_object_id",
                table: "device",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_dfz_device_id",
                table: "dfz",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_dz_device_id",
                table: "dz",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_dzl_device_id",
                table: "dzl",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_mtz_busbar_device_id",
                table: "mtz_busbar",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_oapv_device_id",
                table: "oapv",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_tapv_device_id",
                table: "tapv",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_vt_device_id",
                table: "vt",
                column: "device_id");

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_action_action_id",
                table: "instruction_request",
                column: "action_id",
                principalTable: "action",
                principalColumn: "action_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_app_user_created_by_user_id",
                table: "instruction_request",
                column: "created_by_user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_object_object_id",
                table: "instruction_request",
                column: "object_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_result_instruction_request_instruction_request_",
                table: "instruction_result",
                column: "instruction_request_id",
                principalTable: "instruction_request",
                principalColumn: "instruction_request_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_object_object_types_object_type_id",
                table: "object",
                column: "object_type_id",
                principalTable: "object_type",
                principalColumn: "object_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_object_substations_substation_id",
                table: "object",
                column: "substation_id",
                principalTable: "substations",
                principalColumn: "substation_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_device_device_id",
                table: "request_param_value",
                column: "device_id",
                principalTable: "device",
                principalColumn: "device_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_dfz_dfz_id",
                table: "request_param_value",
                column: "dfz_id",
                principalTable: "dfz",
                principalColumn: "dfz_id");

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_dz_dz_id",
                table: "request_param_value",
                column: "dz_id",
                principalTable: "dz",
                principalColumn: "dz_id");

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_dzl_dzl_id",
                table: "request_param_value",
                column: "dzl_id",
                principalTable: "dzl",
                principalColumn: "dzl_id");

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_instruction_request_instruction_request",
                table: "request_param_value",
                column: "instruction_request_id",
                principalTable: "instruction_request",
                principalColumn: "instruction_request_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_mtz_busbar_mtz_busbar_id",
                table: "request_param_value",
                column: "mtz_busbar_id",
                principalTable: "mtz_busbar",
                principalColumn: "mtz_busbar_id");

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_oapv_oapv_id",
                table: "request_param_value",
                column: "oapv_id",
                principalTable: "oapv",
                principalColumn: "oapv_id");

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_tapvs_tapv_id",
                table: "request_param_value",
                column: "tapv_id",
                principalTable: "tapv",
                principalColumn: "tapv_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_action_action_id",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_app_user_created_by_user_id",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_request_object_object_id",
                table: "instruction_request");

            migrationBuilder.DropForeignKey(
                name: "fk_instruction_result_instruction_request_instruction_request_",
                table: "instruction_result");

            migrationBuilder.DropForeignKey(
                name: "fk_object_object_types_object_type_id",
                table: "object");

            migrationBuilder.DropForeignKey(
                name: "fk_object_substations_substation_id",
                table: "object");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_device_device_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_dfz_dfz_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_dz_dz_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_dzl_dzl_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_instruction_request_instruction_request",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_mtz_busbar_mtz_busbar_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_oapv_oapv_id",
                table: "request_param_value");

            migrationBuilder.DropForeignKey(
                name: "fk_request_param_value_tapvs_tapv_id",
                table: "request_param_value");

            migrationBuilder.DropTable(
                name: "ct_place");

            migrationBuilder.DropTable(
                name: "dfz");

            migrationBuilder.DropTable(
                name: "dz");

            migrationBuilder.DropTable(
                name: "dzl");

            migrationBuilder.DropTable(
                name: "mtz_busbar");

            migrationBuilder.DropTable(
                name: "oapv");

            migrationBuilder.DropTable(
                name: "substations");

            migrationBuilder.DropTable(
                name: "tapv");

            migrationBuilder.DropTable(
                name: "vt");

            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropPrimaryKey(
                name: "pk_request_param_value",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_dfz_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_dz_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_dzl_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_instruction_request_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_mtz_busbar_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_oapv_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_request_param_value_tapv_id",
                table: "request_param_value");

            migrationBuilder.DropIndex(
                name: "ix_object_object_type_id",
                table: "object");

            migrationBuilder.DropIndex(
                name: "ix_object_substation_id",
                table: "object");

            migrationBuilder.DropIndex(
                name: "ix_instruction_request_object_id",
                table: "instruction_request");

            migrationBuilder.DropColumn(
                name: "request_param_value_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "dfz_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "dz_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "dzl_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "mtz_busbar_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "oapv_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "tapv_id",
                table: "request_param_value");

            migrationBuilder.DropColumn(
                name: "substation_id",
                table: "object");

            migrationBuilder.RenameColumn(
                name: "device_id",
                table: "request_param_value",
                newName: "param_definition_id");

            migrationBuilder.RenameIndex(
                name: "ix_request_param_value_device_id",
                table: "request_param_value",
                newName: "ix_request_param_value_param_definition_id");

            migrationBuilder.AddColumn<bool>(
                name: "value_bool",
                table: "request_param_value",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "value_decimal",
                table: "request_param_value",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "value_int_enum",
                table: "request_param_value",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "value_json",
                table: "request_param_value",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "value_origin",
                table: "request_param_value",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "value_text",
                table: "request_param_value",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "object",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "instruction_request",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.AddPrimaryKey(
                name: "pk_request_param_value",
                table: "request_param_value",
                columns: new[] { "instruction_request_id", "param_definition_id" });

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
                name: "param_definition",
                columns: table => new
                {
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    param_domain_id = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: true),
                    value_type = table.Column<string>(type: "text", nullable: false)
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
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    value_name = table.Column<string>(type: "text", nullable: false)
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
                name: "action_param_requirement",
                columns: table => new
                {
                    action_id = table.Column<long>(type: "bigint", nullable: false),
                    param_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ui_prompt = table.Column<string>(type: "text", nullable: true),
                    validation_rule = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    value_kind = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_param_requirement", x => new { x.action_id, x.param_definition_id });
                    table.CheckConstraint("ck_action_param_requirement_sort_order", "sort_order >= 0");
                    table.CheckConstraint("ck_action_param_requirement_value_kind", "value_kind IN ('CONSTANT','VARIABLE')");
                    table.ForeignKey(
                        name: "fk_action_param_requirement_action",
                        column: x => x.action_id,
                        principalTable: "action",
                        principalColumn: "action_id",
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
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    value_bool = table.Column<bool>(type: "boolean", nullable: true),
                    value_decimal = table.Column<decimal>(type: "numeric", nullable: true),
                    value_int_enum = table.Column<int>(type: "integer", nullable: true),
                    value_json = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    value_text = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.AddCheckConstraint(
                name: "ck_request_param_value_origin",
                table: "request_param_value",
                sql: "value_origin IN ('CONSTANT_OBJECT','USER_INPUT','SCADA','CALCULATED')");

            migrationBuilder.AddCheckConstraint(
                name: "ck_request_param_value_single_value",
                table: "request_param_value",
                sql: "num_nonnulls(value_bool, value_int_enum, value_decimal, value_text, value_json) <= 1");

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

            migrationBuilder.AddCheckConstraint(
                name: "ck_instruction_request_status",
                table: "instruction_request",
                sql: "status IN ('DRAFT','CALCULATED','SAVED','ERROR')");

            migrationBuilder.CreateIndex(
                name: "uq_app_user_login",
                table: "app_user",
                column: "login",
                unique: true);

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
                columns: new[] { "action_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_line_rza_device_device",
                table: "line_rza_device",
                column: "device_object_id");

            migrationBuilder.CreateIndex(
                name: "ix_line_rza_device_line",
                table: "line_rza_device",
                column: "line_object_id");

            migrationBuilder.CreateIndex(
                name: "ix_object_param_value_param_definition_id",
                table: "object_param_value",
                column: "param_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_object_param_value_updated_by_user_id",
                table: "object_param_value",
                column: "updated_by_user_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_action",
                table: "instruction_request",
                column: "action_id",
                principalTable: "action",
                principalColumn: "action_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_created_by",
                table: "instruction_request",
                column: "created_by_user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_request_object",
                table: "instruction_request",
                column: "object_id",
                principalTable: "object",
                principalColumn: "object_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_instruction_result_request",
                table: "instruction_result",
                column: "instruction_request_id",
                principalTable: "instruction_request",
                principalColumn: "instruction_request_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_object_object_type",
                table: "object",
                column: "object_type_id",
                principalTable: "object_type",
                principalColumn: "object_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_param_definition",
                table: "request_param_value",
                column: "param_definition_id",
                principalTable: "param_definition",
                principalColumn: "param_definition_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_request_param_value_request",
                table: "request_param_value",
                column: "instruction_request_id",
                principalTable: "instruction_request",
                principalColumn: "instruction_request_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
