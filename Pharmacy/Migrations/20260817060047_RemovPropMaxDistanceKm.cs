using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class RemovPropMaxDistanceKm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDistanceKm",
                table: "Pharmacies");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_PurchaseItemId",
                table: "ProductBatches",
                column: "PurchaseItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatches_PurchaseItems_PurchaseItemId",
                table: "ProductBatches",
                column: "PurchaseItemId",
                principalTable: "PurchaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatches_PurchaseItems_PurchaseItemId",
                table: "ProductBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProductBatches_PurchaseItemId",
                table: "ProductBatches");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxDistanceKm",
                table: "Pharmacies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
