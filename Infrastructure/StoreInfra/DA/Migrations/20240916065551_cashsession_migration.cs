using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class cashsession_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "272e3baa-ae7d-4355-80f0-3d29e1985c71");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a7dabf7f-9718-4587-80cf-b678ec8c9853");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "d41523b9-e04e-4a41-8658-7c40853d71c5");

            migrationBuilder.CreateTable(
                name: "CashSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TillId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<double>(type: "double precision", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CashSessionId = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashDetails_CashSessions_CashSessionId",
                        column: x => x.CashSessionId,
                        principalTable: "CashSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { "7ba6223a-ad11-4669-81e8-1a51b796c3a3", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2875), true, false, "Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "d75a37a5-9126-4fe2-8c5f-80d81d2e6d73", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2887), true, false, "Gift Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ee22a879-d689-4cac-8e50-2472be3408ac", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2725), true, false, "Cash", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashDetails_CashSessionId",
                table: "CashDetails",
                column: "CashSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashDetails");

            migrationBuilder.DropTable(
                name: "CashSessions");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "7ba6223a-ad11-4669-81e8-1a51b796c3a3");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "d75a37a5-9126-4fe2-8c5f-80d81d2e6d73");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "ee22a879-d689-4cac-8e50-2472be3408ac");

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { "272e3baa-ae7d-4355-80f0-3d29e1985c71", "Default", new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7658), true, false, "Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "a7dabf7f-9718-4587-80cf-b678ec8c9853", "Default", new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7597), true, false, "Cash", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "d41523b9-e04e-4a41-8658-7c40853d71c5", "Default", new DateTime(2024, 9, 13, 14, 12, 24, 192, DateTimeKind.Utc).AddTicks(7664), true, false, "Gift Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
