using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecenaSoluciones.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class newSaleColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CashAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepositReference",
                table: "Sales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositsAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TCAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TCReference",
                table: "Sales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DepositReference",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DepositsAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TCAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TCReference",
                table: "Sales");
        }
    }
}
