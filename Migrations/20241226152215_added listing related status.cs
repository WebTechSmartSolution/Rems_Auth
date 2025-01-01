using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class addedlistingrelatedstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("928f079e-3e98-4357-bf5e-c55bb19fd4c0"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Listings");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("928f079e-3e98-4357-bf5e-c55bb19fd4c0"), new DateTime(2024, 12, 22, 12, 15, 4, 88, DateTimeKind.Utc).AddTicks(6232), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }
    }
}
