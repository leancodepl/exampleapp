using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddKratosIdentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KratosIdentities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SchemaId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Traits = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    MetadataPublic = table.Column<JsonElement>(type: "jsonb", nullable: true),
                    MetadataAdmin = table.Column<JsonElement>(type: "jsonb", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KratosIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KratosIdentityRecoveryAddresses",
                schema: "dbo",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Via = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KratosIdentityRecoveryAddresses", x => new { x.IdentityId, x.Id });
                    table.ForeignKey(
                        name: "FK_KratosIdentityRecoveryAddresses_KratosIdentities_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "dbo",
                        principalTable: "KratosIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KratosIdentityVerifiableAddresses",
                schema: "dbo",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Via = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KratosIdentityVerifiableAddresses", x => new { x.IdentityId, x.Id });
                    table.ForeignKey(
                        name: "FK_KratosIdentityVerifiableAddresses_KratosIdentities_Identity~",
                        column: x => x.IdentityId,
                        principalSchema: "dbo",
                        principalTable: "KratosIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KratosIdentityRecoveryAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "KratosIdentityVerifiableAddresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "KratosIdentities",
                schema: "dbo");
        }
    }
}
