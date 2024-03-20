using DecenaSoluciones.POS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Helper
{
    public static class Utility
    {
        public static string GenerateReceiptHtml(AddEditSale sale, string htmlTemplate, string companyName = "")
        {
            string productsHTML = string.Empty;
            string totalTaxes = ToMoneyString(sale.SaleProducts!.Sum(p => p.ITBIS));
            string subTotal = ToMoneyString(sale.SaleProducts!.Sum(p => p.Total) - sale.SaleProducts!.Sum(p => p.ITBIS));

            int count = 1;
            int page = 1;
            foreach (var product in sale.SaleProducts!)
            {
                productsHTML += $"<tr><td>{count}</td>" +
                                $"<td>{product.ProductDescription}</td>" +
                                $"<td>{product.Quantity}</td>" +
                                $"<td>{ToMoneyString(product.UnitPrice)}</td>" +
                                $"<td>{ToMoneyString(product.ITBIS)}</td>" +
                                $"<td>{ToMoneyString(product.Total)}</td></tr>";
                count++;

                if (sale.SaleProducts.Count >= count && (count + 10) % 40 == 1)
                {
                    page++;
                    productsHTML += GetPageBreak();
                }
            }

            if (sale.WorkForceValue != null && sale.WorkForceValue > 0)
            {
                productsHTML += $"<tr><td>000</td><td>Mano de Obra</td><td>000</td>" +
                                $"<td>-</td><td>-</td><td>{ToMoneyString(sale.WorkForceValue)}</td></tr>";
            }

            var lastPageCount = page > 1 ? sale.SaleProducts!.Skip(30 + ((page - 2) * 40)).Count() : 0;
            if ((page == 1 && count >= 20) || (page > 1 && lastPageCount >= 29))
            {
                productsHTML += GetPageBreak();
            }

            htmlTemplate = htmlTemplate.Replace("{{SaleTitle}}", sale.IsAQuotation ? "COTIZACIÓN" : "FACTURA");
            htmlTemplate = htmlTemplate.Replace("{{SaleCode}}", sale.Code);
            htmlTemplate = htmlTemplate.Replace("{{CompanyName}}", companyName);
            htmlTemplate = htmlTemplate.Replace("{{ClientName}}", (sale.Customer!.Name ?? "MOSTRADOR"));
            htmlTemplate = htmlTemplate.Replace("{{SubTotal}}", subTotal);
            htmlTemplate = htmlTemplate.Replace("{{totalTaxes}}", totalTaxes);
            htmlTemplate = htmlTemplate.Replace("{{Discount}}", ToMoneyString(sale.Discount));
            htmlTemplate = htmlTemplate.Replace("{{GrandTotal}}", GetTotalAmount(sale));
            htmlTemplate = htmlTemplate.Replace("{{Products}}", productsHTML);
            htmlTemplate = htmlTemplate.Replace("{{CreationDate}}", DateTime.Now.ToString("dd/MM/yyyy"));

            string payCondition = sale.CreditSale == true ? "CRÉDITO" : "CONTADO";
            htmlTemplate = htmlTemplate.Replace("{{PaymentConditions}}", sale.IsAQuotation ? "" :
                $"<p class=\"ClientP\"><span style=\"font-weight: bold; font-size: 18px\">Condiciones de pago: </span>{payCondition}</p>");

            return htmlTemplate;
        }
        private static string GetPageBreak() => "</table><p style=\"page-break-before: always\">" +
                            "<table class=\"tblProducts\">" +
                            "<tr><th>ITEMS</th>" +
                            "<th>PRODUCTO</th>" +
                            "<th>CANT.</th>" +
                            "<th>PRECIO</th>" +
                            "<th>ITBIS</th>" +
                            "<th>TOTAL</th></tr>";

        private static string GetTotalAmount(AddEditSale sale)
        {
            decimal calc = 0.0M;
            calc += sale.SaleProducts!.Sum(p => p.Total);
            calc += sale.WorkForceValue ?? 0.0M;
            calc -= sale.Discount ?? 0.0M;
            return ToMoneyString(calc);
        }

        private static string ToMoneyString(decimal? value) => value.HasValue ? $"{value.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"))}" : "0.00";
    }
}
