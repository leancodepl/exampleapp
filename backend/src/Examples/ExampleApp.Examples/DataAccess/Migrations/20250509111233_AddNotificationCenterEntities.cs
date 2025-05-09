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
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<string>(type: "character(34)", fixedLength: true, maxLength: 34, nullable: false),
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
                table.PrimaryKey("PK_Messages", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "UserData<Guid>",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                LastReadTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserData<Guid>", x => x.UserId);
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Messages_UserId_DateCreated_Id",
            table: "Messages",
            columns: new[] { "UserId", "DateCreated", "Id" }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Messages");

        migrationBuilder.DropTable(name: "UserData<Guid>");
    }
}
