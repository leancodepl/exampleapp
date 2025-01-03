using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Examples.DataAccess.Migrations;

/// <inheritdoc />
public partial class RenameServiceProviderPhotos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "PromotionalBanner", table: "ServiceProviders", newName: "CoverPhoto");
        migrationBuilder.RenameColumn(name: "ListItemPicture", table: "ServiceProviders", newName: "Thumbnail");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "CoverPhoto", table: "ServiceProviders", newName: "PromotionalBanner");
        migrationBuilder.RenameColumn(name: "Thumbnail", table: "ServiceProviders", newName: "ListItemPicture");
    }
}
