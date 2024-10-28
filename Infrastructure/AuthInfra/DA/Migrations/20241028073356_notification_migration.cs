using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class notification_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    ActionId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleActions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Credential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credential_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "userRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_userRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), true, false, "SuperAdmin", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Email", "IsActive", "IsArchived", "Name", "UpdatedBy", "UpdatedDate", "UserType" },
                values: new object[] { "72990663-2edc-4c10-b331-cd1c65e477e0", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), "POS@gmail.com", true, false, "POS", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "Credential",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "PasswordHash", "PasswordSalt", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { "10bcc73c-e40a-4852-a81d-c18d654e8806", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), true, false, "V3Ti3fXk2SJt8IPQ9t/w4cSIp977mWhP0WLr+q84I7U=", "Tb9GGpb1yGncYSRYkj/Tpg==", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), "72990663-2edc-4c10-b331-cd1c65e477e0" });

            migrationBuilder.InsertData(
                table: "RefereshToken",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ExpireyDate", "IsActive", "IsArchived", "RevokeAble", "Token", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { "f3df99c7-07fb-4b7f-89e5-89d86b84bd41", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), new DateTime(2024, 10, 28, 9, 13, 54, 979, DateTimeKind.Utc).AddTicks(4202), true, false, true, "72990663-2edc-4c10-b331-cd1c65e477e0", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), "72990663-2edc-4c10-b331-cd1c65e477e0" });

            migrationBuilder.InsertData(
                table: "userRoles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "RoleId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { "f3df99c7-07fb-4b7f-89e5-89d86b84bd4e", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), true, false, "78f4b56a-3fa3-4067-b641-7adb0a7a2ca7", "72990663-2edc-4c10-b331-cd1c65e477e0", new DateTime(2024, 10, 28, 7, 33, 54, 968, DateTimeKind.Utc).AddTicks(3013), "72990663-2edc-4c10-b331-cd1c65e477e0" });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefereshToken_UserId",
                table: "RefereshToken",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleActions_ActionId",
                table: "RoleActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActions_RoleId",
                table: "RoleActions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_userRoles_RoleId",
                table: "userRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_userRoles_UserId",
                table: "userRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.DropTable(
                name: "RefereshToken");

            migrationBuilder.DropTable(
                name: "RoleActions");

            migrationBuilder.DropTable(
                name: "userRoles");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
