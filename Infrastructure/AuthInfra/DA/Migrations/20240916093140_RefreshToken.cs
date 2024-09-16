using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefereshToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpireyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokeAble = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefereshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefereshToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), "Arq5AJX3l7UfGe8/xX3vXfLFY1CbOewFMEXJum5Yseo=", "EXLlrXqolrz7P449o9UtAA==", new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.InsertData(
                table: "RefereshToken",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ExpireyDate", "IsActive", "IsArchived", "RevokeAble", "Token", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { "f3df99c7-07fb-4b7f-89e5-89d86b84bd41", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), new DateTime(2024, 9, 16, 11, 11, 39, 846, DateTimeKind.Utc).AddTicks(4206), true, false, true, "72990663-2edc-4c10-b331-cd1c65e477e0", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72990663-2edc-4c10-b331-cd1c65e477e0",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "userRoles",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.CreateIndex(
                name: "IX_RefereshToken_UserId",
                table: "RefereshToken",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefereshToken");

            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472), "d9eaIWSBDoLwkZMUgEdNzYyiwHFg1rcR3gZRlRVYIDQ=", "TzYRkd2fsMUus0DoHl4Dmw==", new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472), new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72990663-2edc-4c10-b331-cd1c65e477e0",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472), new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472) });

            migrationBuilder.UpdateData(
                table: "userRoles",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472), new DateTime(2024, 9, 16, 7, 57, 0, 692, DateTimeKind.Utc).AddTicks(6472) });
        }
    }
}
