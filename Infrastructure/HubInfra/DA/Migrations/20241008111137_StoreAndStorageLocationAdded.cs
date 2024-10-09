using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class StoreAndStorageLocationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_InventoryGroups_GroupId",
                table: "InventoryItems");

            migrationBuilder.DropTable(
                name: "InventoryGroups");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_GroupId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "InventoryItems",
                newName: "TaxType");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "InventoryItems",
                newName: "TaxCode");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "InventoryItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "InventoryItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "BuyUom",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvUom",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryItem",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBatch",
                table: "InventoryItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSerial",
                table: "InventoryItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ItemGroupCode",
                table: "InventoryItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseItem",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellItem",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellUom",
                table: "InventoryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UgpEntry",
                table: "InventoryItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StorageLocation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ERPCode = table.Column<string>(type: "text", nullable: false),
                    ERPName = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    LocCode = table.Column<string>(type: "text", nullable: false),
                    CompanyCode = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    StoreCode = table.Column<string>(type: "text", nullable: false),
                    StoreName = table.Column<string>(type: "text", nullable: false),
                    StorageLocationId = table.Column<string>(type: "text", nullable: false),
                    PriceListId = table.Column<string>(type: "text", nullable: false),
                    AdminUser = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_StorageLocation_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "StorageLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_StorageLocationId",
                table: "Store",
                column: "StorageLocationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "StorageLocation");

            migrationBuilder.DropColumn(
                name: "BuyUom",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "InvUom",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "InventoryItem",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "IsBatch",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "IsSerial",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "ItemGroupCode",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "PurchaseItem",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "SellItem",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "SellUom",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "UgpEntry",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "TaxType",
                table: "InventoryItems",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "InventoryItems",
                newName: "Category");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "InventoryItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemCode",
                table: "InventoryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InventoryGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_GroupId",
                table: "InventoryItems",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_InventoryGroups_GroupId",
                table: "InventoryItems",
                column: "GroupId",
                principalTable: "InventoryGroups",
                principalColumn: "Id");
        }
    }
}
