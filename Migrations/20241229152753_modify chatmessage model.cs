using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class modifychatmessagemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("82eab452-be8c-4ca5-9e06-431f7481be84"));

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("c083770e-2f0f-4d8c-a178-7f67817d1520"), new DateTime(2024, 12, 29, 15, 27, 52, 553, DateTimeKind.Utc).AddTicks(6383), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("c083770e-2f0f-4d8c-a178-7f67817d1520"));

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("82eab452-be8c-4ca5-9e06-431f7481be84"), new DateTime(2024, 12, 29, 13, 57, 59, 862, DateTimeKind.Utc).AddTicks(2067), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }
    }
}
