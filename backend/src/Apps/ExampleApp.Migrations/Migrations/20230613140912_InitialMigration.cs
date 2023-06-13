using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "dbo");

            migrationBuilder.CreateTable(
                name: "ConsumedMessages",
                schema: "dbo",
                columns: table =>
                    new
                    {
                        MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                        ConsumerType = table.Column<string>(
                            type: "character varying(500)",
                            maxLength: 500,
                            nullable: false
                        ),
                        DateConsumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                        MessageType = table.Column<string>(
                            type: "character varying(500)",
                            maxLength: 500,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedMessages", x => new { x.MessageId, x.ConsumerType });
                }
            );

            migrationBuilder.CreateTable(
                name: "RaisedEvents",
                schema: "dbo",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        DateOcurred = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                        EventType = table.Column<string>(
                            type: "character varying(500)",
                            maxLength: 500,
                            nullable: false
                        ),
                        Payload = table.Column<string>(type: "text", nullable: false),
                        Metadata_ActivityContext = table.Column<string>(type: "text", nullable: true),
                        Metadata_ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                        WasPublished = table.Column<bool>(type: "boolean", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaisedEvents", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedMessages_DateConsumed",
                schema: "dbo",
                table: "ConsumedMessages",
                column: "DateConsumed"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RaisedEvents_DateOcurred_WasPublished",
                schema: "dbo",
                table: "RaisedEvents",
                columns: new[] { "DateOcurred", "WasPublished" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ConsumedMessages", schema: "dbo");

            migrationBuilder.DropTable(name: "RaisedEvents", schema: "dbo");
        }
    }
}
