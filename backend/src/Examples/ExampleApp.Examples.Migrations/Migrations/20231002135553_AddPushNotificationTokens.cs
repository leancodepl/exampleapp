using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.Migrations.Migrations;

/// <inheritdoc />
public partial class AddPushNotificationTokens : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PushNotificationTokens",
            columns: table =>
                new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
            constraints: table =>
            {
                table.PrimaryKey("PK_PushNotificationTokens", x => new { x.UserId, x.Token });
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_PushNotificationTokens_Token",
            table: "PushNotificationTokens",
            column: "Token",
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "PushNotificationTokens");
    }
}
