using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class updateduserresponce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Users_UserId1",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_UserId1",
                table: "Listings");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("1d168d20-4cd4-4b7c-b21f-854ba893a107"));

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Listings");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("3d98c9fc-02b4-449f-b16b-a57256b72daa"), new DateTime(2025, 1, 22, 13, 26, 58, 985, DateTimeKind.Utc).AddTicks(9654), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("3d98c9fc-02b4-449f-b16b-a57256b72daa"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Listings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("1d168d20-4cd4-4b7c-b21f-854ba893a107"), new DateTime(2025, 1, 3, 12, 45, 55, 506, DateTimeKind.Utc).AddTicks(8050), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId1",
                table: "Listings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_UserId1",
                table: "Listings",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
