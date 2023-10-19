using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecenaSoluciones.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class CreditSaleField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreditSale",
                table: "Sales",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditSale",
                table: "Sales");
        }
    }
}
