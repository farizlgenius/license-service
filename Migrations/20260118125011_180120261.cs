using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LicenseService.Migrations
{
    /// <inheritdoc />
    public partial class _180120261 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sign_key",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sign_key_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    sign_pub = table.Column<byte[]>(type: "bytea", nullable: false),
                    sign_priv = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sign_key", x => x.id);
                    table.UniqueConstraint("AK_sign_key_sign_key_uuid", x => x.sign_key_uuid);
                });

            migrationBuilder.CreateTable(
                name: "license",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company = table.Column<string>(type: "text", nullable: false),
                    customer_site = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    license = table.Column<byte[]>(type: "bytea", nullable: false),
                    license_type = table.Column<int>(type: "integer", nullable: false),
                    sign_key_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_license", x => x.id);
                    table.ForeignKey(
                        name: "FK_license_sign_key_sign_key_uuid",
                        column: x => x.sign_key_uuid,
                        principalTable: "sign_key",
                        principalColumn: "sign_key_uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_license_sign_key_uuid",
                table: "license",
                column: "sign_key_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "license");

            migrationBuilder.DropTable(
                name: "sign_key");
        }
    }
}
