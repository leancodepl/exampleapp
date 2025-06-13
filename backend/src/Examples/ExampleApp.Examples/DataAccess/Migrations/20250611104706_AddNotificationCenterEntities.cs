using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddNotificationCenterEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<string>(type: "character(39)", fixedLength: true, maxLength: 39, nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Content = table.Column<string>(type: "text", nullable: false),
                Title = table.Column<string>(type: "text", nullable: false),
                Image = table.Column<string>(type: "text", nullable: true),
                DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Payload = table.Column<JsonElement>(type: "jsonb", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "NotificationsUsers",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                LastReadTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NotificationsUsers", x => x.UserId);
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId_DateCreated_Id",
            table: "Notifications",
            columns: new[] { "UserId", "DateCreated", "Id" }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Notifications");

        migrationBuilder.DropTable(name: "NotificationsUsers");
    }
}
