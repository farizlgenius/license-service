using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LicenseService.Migrations
{
    /// <inheritdoc />
    public partial class _120120261 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyPairs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    public_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    secret_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPairs", x => x.id);
                    table.UniqueConstraint("AK_KeyPairs_key_uuid", x => x.key_uuid);
                });

            migrationBuilder.CreateTable(
                name: "Secrets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    secret_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    key_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    public_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    shared_secret = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secrets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Secrets_KeyPairs_key_uuid",
                        column: x => x.key_uuid,
                        principalTable: "KeyPairs",
                        principalColumn: "key_uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company = table.Column<string>(type: "text", nullable: false),
                    customer_site = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    secret_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    license = table.Column<byte[]>(type: "bytea", nullable: false),
                    license_type = table.Column<int>(type: "integer", nullable: false),
                    secret_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Licenses_Secrets_secret_id",
                        column: x => x.secret_id,
                        principalTable: "Secrets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_secret_id",
                table: "Licenses",
                column: "secret_id");

            migrationBuilder.CreateIndex(
                name: "IX_Secrets_key_uuid",
                table: "Secrets",
                column: "key_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Secrets");

            migrationBuilder.DropTable(
                name: "KeyPairs");
        }
    }
}
