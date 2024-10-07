using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class InventoryGroupTableAndRelationsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_ItemId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "ItemGroupCode",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "ItemGroupId",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemsId",
                table: "ItemImages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    GroupCode = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ItemId",
                table: "Taxes",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemGroupId",
                table: "Items",
                column: "ItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemsId",
                table: "ItemImages",
                column: "ItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_Items_ItemsId",
                table: "ItemImages",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemGroups_ItemGroupId",
                table: "Items",
                column: "ItemGroupId",
                principalTable: "ItemGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_Items_ItemsId",
                table: "ItemImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemGroups_ItemGroupId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemGroups");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_ItemId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemGroupId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_ItemImages_ItemsId",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "ItemGroupId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemsId",
                table: "ItemImages");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "Taxes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemGroupCode",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ItemId",
                table: "Taxes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_Items_ItemId",
                table: "ItemImages",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
