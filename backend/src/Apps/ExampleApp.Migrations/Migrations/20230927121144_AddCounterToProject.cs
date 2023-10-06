﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Migrations.Migrations;

/// <inheritdoc />
public partial class AddCounterToProject : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Counter",
            table: "Projects",
            type: "integer",
            nullable: false,
            defaultValue: 0
        );

        migrationBuilder.AddColumn<string>(
            name: "MessageType",
            table: "OutboxMessage",
            type: "text",
            nullable: false,
            defaultValue: ""
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Counter", table: "Projects");

        migrationBuilder.DropColumn(name: "MessageType", table: "OutboxMessage");
    }
}
