using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class bugfixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalArea",
                table: "Listings",
                newName: "PropertyType");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Listings",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "PropertyType",
                table: "Listings",
                newName: "TotalArea");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Listings",
                newName: "Description");
        }
    }
}
