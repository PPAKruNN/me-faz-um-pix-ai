using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fazumpix.Migrations
{
    /// <inheritdoc />
    public partial class create_BTree_index_on_UserCPF_PaymentProviderToken_and_PixKeyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_CPF",
                table: "User",
                column: "CPF");

            migrationBuilder.CreateIndex(
                name: "IX_PixKey_Type",
                table: "PixKey",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentProvider_Token",
                table: "PaymentProvider",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_CPF",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_PixKey_Type",
                table: "PixKey");

            migrationBuilder.DropIndex(
                name: "IX_PaymentProvider_Token",
                table: "PaymentProvider");
        }
    }
}
