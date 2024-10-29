using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class addNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TargetNamespace = table.Column<string>(type: "text", nullable: false),
                    SendToUserType = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSeen",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OnClickDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    By = table.Column<string>(type: "text", nullable: false),
                    NotificationId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSeen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSeen_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260), "dTO+G05LJ3jW+Qkha4FYtQM5AG7hxPDM8/c6YJKYST4=", "xZh0JAO/h8gD2dyrFOFiIw==", new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                table: "RefereshToken",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd41",
                columns: new[] { "CreatedDate", "ExpireyDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260), new DateTime(2024, 10, 29, 9, 27, 20, 303, DateTimeKind.Utc).AddTicks(6553), new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260), new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72990663-2edc-4c10-b331-cd1c65e477e0",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260), new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                table: "userRoles",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260), new DateTime(2024, 10, 29, 7, 47, 20, 293, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSeen_NotificationId",
                table: "NotificationSeen",
                column: "NotificationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSeen");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.UpdateData(
                table: "Credential",
                keyColumn: "Id",
                keyValue: "10bcc73c-e40a-4852-a81d-c18d654e8806",
                columns: new[] { "CreatedDate", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151), "p+NGQLMLa+q4EefeX2uCigz7IuRvaVp6nY0rSO63+5U=", "yfSEIBGd/VPKqKyT6UHopA==", new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "RefereshToken",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd41",
                columns: new[] { "CreatedDate", "ExpireyDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151), new DateTime(2024, 10, 29, 9, 20, 10, 812, DateTimeKind.Utc).AddTicks(262), new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151), new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72990663-2edc-4c10-b331-cd1c65e477e0",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151), new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "userRoles",
                keyColumn: "Id",
                keyValue: "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e",
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151), new DateTime(2024, 10, 29, 7, 40, 10, 801, DateTimeKind.Utc).AddTicks(1151) });
        }
    }
}
