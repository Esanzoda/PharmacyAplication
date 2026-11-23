using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class RenameProoertyOrderItemIdToOrderItemEntityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemBatches_OrderItems_OrderItemEntityId",
                table: "OrderItemBatches");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "OrderItemBatches");

            migrationBuilder.AlterColumn<long>(
                name: "OrderItemEntityId",
                table: "OrderItemBatches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemBatches_OrderItems_OrderItemEntityId",
                table: "OrderItemBatches",
                column: "OrderItemEntityId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemBatches_OrderItems_OrderItemEntityId",
                table: "OrderItemBatches");

            migrationBuilder.AlterColumn<long>(
                name: "OrderItemEntityId",
                table: "OrderItemBatches",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "OrderItemId",
                table: "OrderItemBatches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemBatches_OrderItems_OrderItemEntityId",
                table: "OrderItemBatches",
                column: "OrderItemEntityId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }
    }
}
