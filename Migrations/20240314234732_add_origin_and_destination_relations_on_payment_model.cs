using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fazumpix.Migrations
{
    /// <inheritdoc />
    public partial class add_origin_and_destination_relations_on_payment_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentProviderAccount_PaymentProviderAccountId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "PaymentProviderAccountId",
                table: "Payment",
                newName: "OriginPaymentProviderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PaymentProviderAccountId",
                table: "Payment",
                newName: "IX_Payment_OriginPaymentProviderAccountId");

            migrationBuilder.AddColumn<long>(
                name: "DestinationPaymentProviderAccountId",
                table: "Payment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_DestinationPaymentProviderAccountId",
                table: "Payment",
                column: "DestinationPaymentProviderAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentProviderAccount_DestinationPaymentProviderAc~",
                table: "Payment",
                column: "DestinationPaymentProviderAccountId",
                principalTable: "PaymentProviderAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentProviderAccount_OriginPaymentProviderAccount~",
                table: "Payment",
                column: "OriginPaymentProviderAccountId",
                principalTable: "PaymentProviderAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentProviderAccount_DestinationPaymentProviderAc~",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentProviderAccount_OriginPaymentProviderAccount~",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_DestinationPaymentProviderAccountId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DestinationPaymentProviderAccountId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "OriginPaymentProviderAccountId",
                table: "Payment",
                newName: "PaymentProviderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_OriginPaymentProviderAccountId",
                table: "Payment",
                newName: "IX_Payment_PaymentProviderAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentProviderAccount_PaymentProviderAccountId",
                table: "Payment",
                column: "PaymentProviderAccountId",
                principalTable: "PaymentProviderAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
