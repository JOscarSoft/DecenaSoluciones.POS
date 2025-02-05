using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecenaSoluciones.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class CustomReceipts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuotationsReceipt",
                table: "Companies",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesReceipt",
                table: "Companies",
                type: "varchar(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuotationsReceipt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SalesReceipt",
                table: "Companies");
        }
    }
}
