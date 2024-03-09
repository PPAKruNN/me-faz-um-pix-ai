using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fazumpix.Migrations
{
    /// <inheritdoc />
    public partial class remove_verification_digits_from_PaymentProviderAccountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgencyVerify",
                table: "PaymentProviderAccount");

            migrationBuilder.DropColumn(
                name: "NumberVerify",
                table: "PaymentProviderAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyVerify",
                table: "PaymentProviderAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NumberVerify",
                table: "PaymentProviderAccount",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
