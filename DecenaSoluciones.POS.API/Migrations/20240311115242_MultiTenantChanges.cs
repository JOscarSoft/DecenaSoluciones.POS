using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecenaSoluciones.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class MultiTenantChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SaleSequence",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "QuotationSequence",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "QuotationProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Quotation",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CustomerProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] {"Name", "Active" },
                values: new object[,]
                {
                        { "Decena Soluciones Tecnicas", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleSequence_CompanyId",
                table: "SaleSequence",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CompanyId",
                table: "Sales",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_CompanyId",
                table: "SaleProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationSequence_CompanyId",
                table: "QuotationSequence",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationProducts_CompanyId",
                table: "QuotationProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_CompanyId",
                table: "Quotation",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CompanyId",
                table: "CustomerProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_Companies_CompanyId",
                table: "CustomerProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Companies_CompanyId",
                table: "Customers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Companies_CompanyId",
                table: "Quotation",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationProducts_Companies_CompanyId",
                table: "QuotationProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationSequence_Companies_CompanyId",
                table: "QuotationSequence",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleProducts_Companies_CompanyId",
                table: "SaleProducts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Companies_CompanyId",
                table: "Sales",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleSequence_Companies_CompanyId",
                table: "SaleSequence",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_Companies_CompanyId",
                table: "CustomerProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Companies_CompanyId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Companies_CompanyId",
                table: "Quotation");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationProducts_Companies_CompanyId",
                table: "QuotationProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationSequence_Companies_CompanyId",
                table: "QuotationSequence");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleProducts_Companies_CompanyId",
                table: "SaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Companies_CompanyId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleSequence_Companies_CompanyId",
                table: "SaleSequence");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_SaleSequence_CompanyId",
                table: "SaleSequence");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CompanyId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_SaleProducts_CompanyId",
                table: "SaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuotationSequence_CompanyId",
                table: "QuotationSequence");

            migrationBuilder.DropIndex(
                name: "IX_QuotationProducts_CompanyId",
                table: "QuotationProducts");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_CompanyId",
                table: "Quotation");

            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProducts_CompanyId",
                table: "CustomerProducts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SaleSequence");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SaleProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "QuotationSequence");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "QuotationProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Quotation");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");
        }
    }
}
