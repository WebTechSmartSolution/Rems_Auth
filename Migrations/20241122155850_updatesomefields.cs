using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class updatesomefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "GarageSizeDescription",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "YearBuilt",
                table: "Listings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GarageSizeDescription",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearBuilt",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
