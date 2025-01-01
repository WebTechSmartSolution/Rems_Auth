using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rems_Auth.Migrations
{
    /// <inheritdoc />
    public partial class addedmessagemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("c083770e-2f0f-4d8c-a178-7f67817d1520"));

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("d98f3887-1263-4fe7-8473-3ea5ca2037aa"), new DateTime(2024, 12, 29, 15, 56, 56, 422, DateTimeKind.Utc).AddTicks(794), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("d98f3887-1263-4fe7-8473-3ea5ca2037aa"));

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { new Guid("c083770e-2f0f-4d8c-a178-7f67817d1520"), new DateTime(2024, 12, 29, 15, 27, 52, 553, DateTimeKind.Utc).AddTicks(6383), "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", null, "admin" });
        }
    }
}
