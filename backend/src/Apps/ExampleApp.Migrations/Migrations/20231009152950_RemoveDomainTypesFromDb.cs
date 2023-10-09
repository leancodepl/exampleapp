using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDomainTypesFromDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "OwnedEntity",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "project_id"
            );

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "IncludedEntity",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "project_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "OwnedEntity",
                type: "project_id",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext"
            );

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "IncludedEntity",
                type: "project_id",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext"
            );
        }
    }
}
