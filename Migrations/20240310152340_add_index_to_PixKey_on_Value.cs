using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fazumpix.Migrations
{
    /// <inheritdoc />
    public partial class add_index_to_PixKey_on_Value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PixKey_Type",
                table: "PixKey");

            migrationBuilder.CreateIndex(
                name: "IX_PixKey_Value",
                table: "PixKey",
                column: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PixKey_Value",
                table: "PixKey");

            migrationBuilder.CreateIndex(
                name: "IX_PixKey_Type",
                table: "PixKey",
                column: "Type");
        }
    }
}
