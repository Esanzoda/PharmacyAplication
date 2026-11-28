using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProductService.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiredProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatches_Products_ProductEntityId",
                table: "ProductBatches");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductBatches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ExpireDateProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalSalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpireDateProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpireDateItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmacyId = table.Column<long>(type: "bigint", nullable: false),
                    ExpiryDateEntityId = table.Column<long>(type: "bigint", nullable: false),
                    ExpiredProductsEntityId = table.Column<long>(type: "bigint", nullable: false),
                    ProductBatchId = table.Column<long>(type: "bigint", nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalSalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpireDateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpireDateItems_ExpireDateProducts_ExpiredProductsEntityId",
                        column: x => x.ExpiredProductsEntityId,
                        principalTable: "ExpireDateProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpireDateItems_ProductBatches_ProductBatchId",
                        column: x => x.ProductBatchId,
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpireDateItems_ExpiredProductsEntityId",
                table: "ExpireDateItems",
                column: "ExpiredProductsEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpireDateItems_ProductBatchId",
                table: "ExpireDateItems",
                column: "ProductBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatches_Products_ProductEntityId",
                table: "ProductBatches",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatches_Products_ProductEntityId",
                table: "ProductBatches");

            migrationBuilder.DropTable(
                name: "ExpireDateItems");

            migrationBuilder.DropTable(
                name: "ExpireDateProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductBatches",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatches_Products_ProductEntityId",
                table: "ProductBatches",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
