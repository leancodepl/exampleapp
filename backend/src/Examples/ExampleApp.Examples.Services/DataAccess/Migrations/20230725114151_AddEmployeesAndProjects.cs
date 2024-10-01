using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.Services.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddEmployeesAndProjects : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase().Annotation("Npgsql:PostgresExtension:citext", ",,");

        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Assignments",
            columns: table => new
            {
                Id = table.Column<string>(type: "citext", nullable: false),
                ParentProjectId = table.Column<string>(type: "citext", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                AssignedEmployeeId = table.Column<string>(type: "citext", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Assignments", x => new { x.ParentProjectId, x.Id });
                table.ForeignKey(
                    name: "FK_Assignments_Projects_ParentProjectId",
                    column: x => x.ParentProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Assignments");

        migrationBuilder.DropTable(name: "Employees");

        migrationBuilder.DropTable(name: "Projects");

        migrationBuilder.AlterDatabase().OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
    }
}
