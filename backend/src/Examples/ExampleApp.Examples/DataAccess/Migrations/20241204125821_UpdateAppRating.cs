using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.DataAccess.Migrations;

/// <inheritdoc />
public partial class UpdateAppRating : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(name: "PK_AppRatings", table: "AppRatings");

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "AppRatings",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
        );

        migrationBuilder.AddPrimaryKey(name: "PK_AppRatings", table: "AppRatings", column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_AppRatings_UserId_DateCreated",
            table: "AppRatings",
            columns: new[] { "UserId", "DateCreated" }
        );

        migrationBuilder.AddForeignKey(
            name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
            table: "OutboxMessage",
            columns: new[] { "InboxMessageId", "InboxConsumerId" },
            principalTable: "InboxState",
            principalColumns: new[] { "MessageId", "ConsumerId" }
        );

        migrationBuilder.AddForeignKey(
            name: "FK_OutboxMessage_OutboxState_OutboxId",
            table: "OutboxMessage",
            column: "OutboxId",
            principalTable: "OutboxState",
            principalColumn: "OutboxId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
            table: "OutboxMessage"
        );

        migrationBuilder.DropForeignKey(name: "FK_OutboxMessage_OutboxState_OutboxId", table: "OutboxMessage");

        migrationBuilder.DropPrimaryKey(name: "PK_AppRatings", table: "AppRatings");

        migrationBuilder.DropIndex(name: "IX_AppRatings_UserId_DateCreated", table: "AppRatings");

        migrationBuilder.DropColumn(name: "Id", table: "AppRatings");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AppRatings",
            table: "AppRatings",
            columns: new[] { "UserId", "DateCreated" }
        );
    }
}
