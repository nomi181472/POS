using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class itemsCartRelationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerCartItems_ItemId",
                table: "CustomerCartItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCartItems_Items_ItemId",
                table: "CustomerCartItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCartItems_Items_ItemId",
                table: "CustomerCartItems");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCartItems_ItemId",
                table: "CustomerCartItems");
        }
    }
}
