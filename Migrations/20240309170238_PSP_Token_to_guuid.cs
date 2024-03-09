using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fazumpix.Migrations
{
    /// <inheritdoc />
    public partial class PSP_Token_to_guuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""PaymentProvider""
                ALTER COLUMN ""Token"" TYPE uuid USING ""Token""::uuid
            ");

            migrationBuilder.AlterColumn<Guid>(
                name: "Token",
                table: "PaymentProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "PaymentProvider",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

        }
    }
}
