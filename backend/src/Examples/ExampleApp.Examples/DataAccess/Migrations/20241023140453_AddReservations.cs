using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.Migrations;

/// <inheritdoc />
public partial class AddReservations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsReserved",
            table: "Timeslots",
            type: "boolean",
            nullable: false,
            defaultValue: false
        );

        migrationBuilder.CreateTable(
            name: "Reservations",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                CalendarDayId = table.Column<string>(type: "citext", nullable: false),
                TimeslotId = table.Column<string>(type: "citext", nullable: false),
                CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reservations", x => x.Id);
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Reservations_CustomerId_Status",
            table: "Reservations",
            columns: new[] { "CustomerId", "Status" }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Reservations_CustomerId_TimeslotId",
            table: "Reservations",
            columns: new[] { "CustomerId", "TimeslotId" }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Reservations");

        migrationBuilder.DropColumn(name: "IsReserved", table: "Timeslots");
    }
}
