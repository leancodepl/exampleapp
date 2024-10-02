using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.Services.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddServiceProviders : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ServiceProviders",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                Description = table.Column<string>(type: "text", nullable: false),
                PromotionalBanner = table.Column<string>(type: "text", nullable: false),
                ListItemPicture = table.Column<string>(type: "text", nullable: false),
                IsPromotionActive = table.Column<bool>(type: "boolean", nullable: false),
                Address = table.Column<string>(type: "text", nullable: false),
                Location_Longitude = table.Column<double>(type: "double precision", nullable: false),
                Location_Latitude = table.Column<double>(type: "double precision", nullable: false),
                DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceProviders", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Timeslots",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                ServiceProviderId = table.Column<string>(type: "citext", nullable: false),
                Date = table.Column<DateOnly>(type: "date", nullable: false),
                StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                Price_Value = table.Column<decimal>(type: "numeric", nullable: false),
                Price_Currency = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Timeslots", x => x.Id);
                table.ForeignKey(
                    name: "FK_Timeslots_ServiceProviders_ServiceProviderId",
                    column: x => x.ServiceProviderId,
                    principalTable: "ServiceProviders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Timeslots_ServiceProviderId",
            table: "Timeslots",
            column: "ServiceProviderId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Timeslots");

        migrationBuilder.DropTable(name: "ServiceProviders");
    }
}
