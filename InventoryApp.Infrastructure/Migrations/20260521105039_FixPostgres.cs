using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Likes",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Items",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "InventoryTags",
                newName: "InventoryTags",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "InventoryFields",
                newName: "InventoryFields",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "InventoryAccesses",
                newName: "InventoryAccesses",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Inventories",
                newName: "Inventories",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "CustomIdFormats",
                newName: "CustomIdFormats",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "CustomIdElements",
                newName: "CustomIdElements",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comments",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "public",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Likes",
                schema: "public",
                newName: "Likes");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "public",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "InventoryTags",
                schema: "public",
                newName: "InventoryTags");

            migrationBuilder.RenameTable(
                name: "InventoryFields",
                schema: "public",
                newName: "InventoryFields");

            migrationBuilder.RenameTable(
                name: "InventoryAccesses",
                schema: "public",
                newName: "InventoryAccesses");

            migrationBuilder.RenameTable(
                name: "Inventories",
                schema: "public",
                newName: "Inventories");

            migrationBuilder.RenameTable(
                name: "CustomIdFormats",
                schema: "public",
                newName: "CustomIdFormats");

            migrationBuilder.RenameTable(
                name: "CustomIdElements",
                schema: "public",
                newName: "CustomIdElements");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "public",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "public",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "public",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "public",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "public",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "public",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "public",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "public",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "public",
                newName: "AspNetRoleClaims");
        }
    }
}
