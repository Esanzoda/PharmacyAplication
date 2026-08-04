using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderPricePropertytoSalePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPrice",
                table: "Products",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "TotalOrderPrice",
                table: "ExpireDateProducts",
                newName: "TotalSalePrice");

            migrationBuilder.RenameColumn(
                name: "TotalOrderPrice",
                table: "ExpireDateItems",
                newName: "TotalSalePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Products",
                newName: "OrderPrice");

            migrationBuilder.RenameColumn(
                name: "TotalSalePrice",
                table: "ExpireDateProducts",
                newName: "TotalOrderPrice");

            migrationBuilder.RenameColumn(
                name: "TotalSalePrice",
                table: "ExpireDateItems",
                newName: "TotalOrderPrice");
        }
    }
}
