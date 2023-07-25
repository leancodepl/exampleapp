using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeesAndProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.Sql(
                """
                create domain employee_id as citext check(value ~ '^employee_[0-7][0-9A-HJKMNP-TV-Z]{25}$');
                create domain project_id as citext check(value ~ '^project_[0-7][0-9A-HJKMNP-TV-Z]{25}$');
                create domain assignment_id as citext check(value ~ '^assignment_[0-7][0-9A-HJKMNP-TV-Z]{25}$');
                """
            );

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "employee_id", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        Email = table.Column<string>(type: "text", nullable: false),
                        DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                        xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "project_id", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                        xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "assignment_id", nullable: false),
                        ParentProjectId = table.Column<string>(type: "project_id", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        Status = table.Column<int>(type: "integer", nullable: false),
                        AssignedEmployeeId = table.Column<string>(type: "employee_id", nullable: true)
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

            migrationBuilder.Sql(
                """
                drop domain employee_id;
                drop domain project_id;
                drop domain assignment_id;
                """
            );

            migrationBuilder.AlterDatabase().OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
        }
    }
}
