using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExampleApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTestEntities : Migration
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

            migrationBuilder.CreateTable(
                name: "IncludedEntity",
                columns: table =>
                    new
                    {
                        ProjectId = table.Column<string>(type: "citext", nullable: false),
                        SomeInt = table.Column<int>(type: "integer", nullable: false),
                        SomeString = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncludedEntity", x => new { x.ProjectId, x.SomeInt });
                    table.ForeignKey(
                        name: "FK_IncludedEntity_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "OwnedEntity",
                columns: table =>
                    new
                    {
                        ProjectId = table.Column<string>(type: "citext", nullable: false),
                        Id = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        SomeInt = table.Column<int>(type: "integer", nullable: false),
                        SomeString = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedEntity", x => new { x.ProjectId, x.Id });
                    table.ForeignKey(
                        name: "FK_OwnedEntity_Projects_ProjectId",
                        column: x => x.ProjectId,
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
            migrationBuilder.DropTable(name: "IncludedEntity");

            migrationBuilder.DropTable(name: "OwnedEntity");

            migrationBuilder.DropColumn(name: "Counter", table: "Projects");
        }
    }
}
