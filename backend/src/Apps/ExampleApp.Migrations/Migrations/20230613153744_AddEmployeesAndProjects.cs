﻿using System;
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
            migrationBuilder.Sql(
                """
                create domain dbo.assignment_id as text collate "ucs_basic" check(value ~ '^assignment_[0-9a-f]{32}$');
                create domain dbo.employee_id as text collate "ucs_basic" check(value ~ '^employee_[0-9a-f]{32}$');
                create domain dbo.project_id as text collate "ucs_basic" check(value ~ '^project_[0-9a-f]{32}$');
                """
            );

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "dbo",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "dbo.employee_id", nullable: false),
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
                schema: "dbo",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "dbo.project_id", nullable: false),
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
                schema: "dbo",
                columns: table =>
                    new
                    {
                        Id = table.Column<string>(type: "dbo.assignment_id", nullable: false),
                        ParentProjectId = table.Column<string>(type: "dbo.project_id", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        Status = table.Column<int>(type: "integer", nullable: false),
                        AssignedEmployeeId = table.Column<string>(type: "dbo.employee_id", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => new { x.ParentProjectId, x.Id });
                    table.ForeignKey(
                        name: "FK_Assignments_Projects_ParentProjectId",
                        column: x => x.ParentProjectId,
                        principalSchema: "dbo",
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
            migrationBuilder.DropTable(name: "Assignments", schema: "dbo");

            migrationBuilder.DropTable(name: "Employees", schema: "dbo");

            migrationBuilder.DropTable(name: "Projects", schema: "dbo");

            migrationBuilder.Sql(
                """
                drop domain dbo.assignment_id;
                drop domain dbo.employee_id;
                drop domain dbo.project_id;
                """
            );
        }
    }
}