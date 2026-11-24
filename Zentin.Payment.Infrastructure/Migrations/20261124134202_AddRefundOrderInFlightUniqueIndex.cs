using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zentin.Payment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefundOrderInFlightUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IdxPayOrderId",
                table: "RefundOrder");

            migrationBuilder.CreateIndex(
                name: "UQ_RefundOrder_InFlight",
                table: "RefundOrder",
                column: "PayOrderId",
                unique: true,
                filter: "[State] IN (0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_RefundOrder_InFlight",
                table: "RefundOrder");

            migrationBuilder.CreateIndex(
                name: "IdxPayOrderId",
                table: "RefundOrder",
                column: "PayOrderId");
        }
    }
}
