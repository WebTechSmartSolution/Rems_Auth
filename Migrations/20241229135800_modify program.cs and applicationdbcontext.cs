using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class modifyprogramcsandapplicationdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("82eab452-be8c-4ca5-9e06-431f7481be84"), new DateTime(2024, 12, 29, 13, 57, 59, 862, DateTimeKind.Utc).AddTicks(2067), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("82eab452-be8c-4ca5-9e06-431f7481be84"));
        }
    }
}
