using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class inventoryTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    ItemName = table.Column<string>(type: "text", nullable: true),
                    ItemGroupCode = table.Column<int>(type: "integer", nullable: true),
                    PurchaseItem = table.Column<string>(type: "text", nullable: true),
                    SellItem = table.Column<string>(type: "text", nullable: true),
                    InventoryItem = table.Column<string>(type: "text", nullable: true),
                    UgpEntry = table.Column<int>(type: "integer", nullable: true),
                    BuyUom = table.Column<string>(type: "text", nullable: true),
                    SellUom = table.Column<string>(type: "text", nullable: true),
                    InvUom = table.Column<string>(type: "text", nullable: true),
                    IsSerial = table.Column<bool>(type: "boolean", nullable: false),
                    IsBatch = table.Column<bool>(type: "boolean", nullable: false),
                    TaxType = table.Column<string>(type: "text", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemImages_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    ItemCode = table.Column<string>(type: "text", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    Percentage = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "647a1871-3dbe-434b-bd2c-877cecdeb964",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "9fabcd90-1a72-4825-b813-f821c088bac6",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a0977129-9878-499e-a912-4156a7ead1c9",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a5fc46ab-35c4-4ac6-9773-e4bdff743f94",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "bb8b2f52-f3da-4704-80e3-db641c337bb5",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "c2d15ca4-04f9-4531-9645-ccfd75429252",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "f69246eb-c27e-4964-bb11-373baab02634",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ItemId",
                table: "Taxes",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemImages");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "647a1871-3dbe-434b-bd2c-877cecdeb964",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(136));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "9fabcd90-1a72-4825-b813-f821c088bac6",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a0977129-9878-499e-a912-4156a7ead1c9",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(132));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "a5fc46ab-35c4-4ac6-9773-e4bdff743f94",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(115));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "bb8b2f52-f3da-4704-80e3-db641c337bb5",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "c2d15ca4-04f9-4531-9645-ccfd75429252",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 613, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: "f69246eb-c27e-4964-bb11-373baab02634",
                column: "CreatedDate",
                value: new DateTime(2024, 9, 16, 11, 31, 0, 612, DateTimeKind.Utc).AddTicks(9906));
        }
    }
}
