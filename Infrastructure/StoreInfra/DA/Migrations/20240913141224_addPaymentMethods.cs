using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
