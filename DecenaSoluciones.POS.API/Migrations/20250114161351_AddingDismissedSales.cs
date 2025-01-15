using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecenaSoluciones.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddingDismissedSales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dismissed",
                table: "Sales",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DismissedBySaleId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_DismissedBySaleId",
                table: "Sales",
                column: "DismissedBySaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Sales_DismissedBySaleId",
                table: "Sales",
                column: "DismissedBySaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Sales_DismissedBySaleId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_DismissedBySaleId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Dismissed",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DismissedBySaleId",
                table: "Sales");
        }
    }
}
