using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class addNewPaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { "647a1871-3dbe-434b-bd2c-877cecdeb964", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(136), true, false, "Loyalty Points", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "9fabcd90-1a72-4825-b813-f821c088bac6", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(140), true, false, "Discount Voucher", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "a0977129-9878-499e-a912-4156a7ead1c9", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(132), true, false, "Online Transfer", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "a5fc46ab-35c4-4ac6-9773-e4bdff743f94", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(115), true, false, "Cheque", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "bb8b2f52-f3da-4704-80e3-db641c337bb5", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(58), true, false, "Gift Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "c2d15ca4-04f9-4531-9645-ccfd75429252", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(53), true, false, "Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "f69246eb-c27e-4964-bb11-373baab02634", "Default", new DateTime(2024, 9, 16, 11, 31, 0, 612, DateTimeKind.Utc).AddTicks(9906), true, false, "Cash", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "647a1871-3dbe-434b-bd2c-877cecdeb964");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "9fabcd90-1a72-4825-b813-f821c088bac6");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a0977129-9878-499e-a912-4156a7ead1c9");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a5fc46ab-35c4-4ac6-9773-e4bdff743f94");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "bb8b2f52-f3da-4704-80e3-db641c337bb5");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "c2d15ca4-04f9-4531-9645-ccfd75429252");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "f69246eb-c27e-4964-bb11-373baab02634");

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "IsArchived", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { "7ba6223a-ad11-4669-81e8-1a51b796c3a3", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2875), true, false, "Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "d75a37a5-9126-4fe2-8c5f-80d81d2e6d73", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2887), true, false, "Gift Card", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ee22a879-d689-4cac-8e50-2472be3408ac", "Default", new DateTime(2024, 9, 16, 6, 55, 49, 50, DateTimeKind.Utc).AddTicks(2725), true, false, "Cash", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
