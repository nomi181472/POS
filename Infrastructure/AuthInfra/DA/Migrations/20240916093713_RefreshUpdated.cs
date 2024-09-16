using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class RefreshUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), "+9+/xVd25XvmTCwGGmd40tnBEzR0pIHnCw62JFNSlp8=", "31Wbh5304Z1mw2O+3ZOSJA==", new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143) });

            migrationBuilder.UpdateData(
                table: "RefereshToken",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd41",
                columns: new[] { "CreatedDate", "ExpireyDate", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), new DateTime(2024, 9, 16, 11, 17, 12, 635, DateTimeKind.Utc).AddTicks(1612), new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), "72990663-2edc-4c10-b331-cd1c65e477e0" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72990663-2edc-4c10-b331-cd1c65e477e0",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143) });

            migrationBuilder.UpdateData(
                table: "userRoles",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143), new DateTime(2024, 9, 16, 9, 37, 12, 625, DateTimeKind.Utc).AddTicks(4143) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), "Arq5AJX3l7UfGe8/xX3vXfLFY1CbOewFMEXJum5Yseo=", "EXLlrXqolrz7P449o9UtAA==", new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "RefereshToken",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd41",
                columns: new[] { "CreatedDate", "ExpireyDate", "UpdatedDate", "UserId" },
                values: new object[] { new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), new DateTime(2024, 9, 16, 11, 11, 39, 846, DateTimeKind.Utc).AddTicks(4206), new DateTime(2024, 9, 16, 9, 31, 39, 833, DateTimeKind.Utc).AddTicks(5739), null });

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
        }
    }
}
