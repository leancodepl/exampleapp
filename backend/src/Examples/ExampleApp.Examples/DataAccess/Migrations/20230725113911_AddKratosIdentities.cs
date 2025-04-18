using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddKratosIdentities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "KratosIdentities",
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
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KratosIdentities", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "KratosIdentityRecoveryAddresses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Via = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KratosIdentityRecoveryAddresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_KratosIdentityRecoveryAddresses_KratosIdentities_IdentityId",
                    column: x => x.IdentityId,
                    principalTable: "KratosIdentities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "KratosIdentityVerifiableAddresses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Via = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KratosIdentityVerifiableAddresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_KratosIdentityVerifiableAddresses_KratosIdentities_Identity~",
                    column: x => x.IdentityId,
                    principalTable: "KratosIdentities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_KratosIdentityRecoveryAddresses_IdentityId",
            table: "KratosIdentityRecoveryAddresses",
            column: "IdentityId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_KratosIdentityVerifiableAddresses_IdentityId",
            table: "KratosIdentityVerifiableAddresses",
            column: "IdentityId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "KratosIdentityRecoveryAddresses");

        migrationBuilder.DropTable(name: "KratosIdentityVerifiableAddresses");

        migrationBuilder.DropTable(name: "KratosIdentities");
    }
}
