using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.DataAccess.Migrations;

/// <inheritdoc />
public partial class ReworkKratosIdentities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Email", table: "KratosIdentities");

        migrationBuilder
            .AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:citext", ",,")
            .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,")
            .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

        migrationBuilder.CreateIndex(
            name: "IX_KratosIdentities_CreatedAt",
            table: "KratosIdentities",
            column: "CreatedAt"
        );

        migrationBuilder.Sql(
            """
            CREATE INDEX "IX_KratosIdentities_Traits_Email"
            ON "KratosIdentities"
            USING gin (("Traits" ->> 'email') gin_trgm_ops);
            """
        );

        migrationBuilder.Sql(
            """
            CREATE INDEX "IX_KratosIdentities_Traits_GivenName"
            ON "KratosIdentities"
            USING gin (("Traits" ->> 'given_name') gin_trgm_ops);
            """
        );

        migrationBuilder.Sql(
            """
            CREATE INDEX "IX_KratosIdentities_Traits_FamilyName"
            ON "KratosIdentities"
            USING gin (("Traits" ->> 'family_name') gin_trgm_ops);
            """
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_KratosIdentities_Traits_Email"";");
        migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_KratosIdentities_Traits_GivenName"";");
        migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_KratosIdentities_Traits_FamilyName"";");

        migrationBuilder.DropIndex(name: "IX_KratosIdentities_CreatedAt", table: "KratosIdentities");

        migrationBuilder
            .AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:citext", ",,")
            .OldAnnotation("Npgsql:PostgresExtension:citext", ",,")
            .OldAnnotation("Npgsql:PostgresExtension:pg_trgm", ",,");

        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "KratosIdentities",
            type: "text",
            nullable: false,
            defaultValue: ""
        );
    }
}
