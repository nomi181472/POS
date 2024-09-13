using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class addTillIdInTill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TillId",
                table: "CustomerCart",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCart_TillId",
                table: "CustomerCart",
                column: "TillId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCart_Till_TillId",
                table: "CustomerCart",
                column: "TillId",
                principalTable: "Till",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCart_Till_TillId",
                table: "CustomerCart");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCart_TillId",
                table: "CustomerCart");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TillId",
                table: "CustomerCart");
        }
    }
}
