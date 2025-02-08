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
        public static string GenerateQuotationReceiptHtml(AddEditSale sale, string htmlTemplate, CompanyViewModel? company, bool useTableBreaks = true)
        {
            string productsHTML = string.Empty;
            string totalTaxes = ToMoneyString(sale.SaleProducts!.Sum(p => p.ITBIS));
            string subTotal = ToMoneyString(sale.SaleProducts!.Sum(p => p.Total) - sale.SaleProducts!.Sum(p => p.ITBIS));

            int count = 1;
            int page = 1;

            string trProducts = ExtractBetweenMarkers(htmlTemplate, "<tr productLine>", "</tr productLine>");
            foreach (var product in sale.SaleProducts!)
            {
                string trItem = trProducts
                    .Replace("{{Product.Index}}", count.ToString())
                    .Replace("{{Product.Code}}", product.ProductCode)
                    .Replace("{{Product.Description}}", product.ProductDescription)
                    .Replace("{{Product.Quantity}}", product.Quantity.ToString())
                    .Replace("{{Product.Price}}", ToMoneyString(product.UnitPrice))
                    .Replace("{{Product.ITBIS}}", ToMoneyString(product.ITBIS))
                    .Replace("{{Product.Amount}}", ToMoneyString(product.Total));


                productsHTML += trItem;
            }



            if (sale.WorkForceValue != null && sale.WorkForceValue > 0)
            {
                productsHTML += $"<tr><td>000</td><td>Mano de Obra</td><td>000</td>" +
                                $"<td>-</td><td>-</td><td>{ToMoneyString(sale.WorkForceValue)}</td></tr>";
            }

            //if (useTableBreaks)
            //{ 
            //    var lastPageCount = page > 1 ? sale.SaleProducts!.Skip(30 + ((page - 2) * 40)).Count() : 0;
            //    if ((page == 1 && count >= 20) || (page > 1 && lastPageCount >= 29))
            //    {
            //        productsHTML += GetPageBreak();
            //    }
            //}

            htmlTemplate = ReplaceRange(htmlTemplate, "<tr productLine>", "</tr productLine>", productsHTML);
            htmlTemplate = htmlTemplate.Replace("{{SaleTitle}}", sale.IsAQuotation ? "COTIZACIÓN" : "FACTURA");
            htmlTemplate = htmlTemplate.Replace("{{SaleCode}}", sale.Code);
            htmlTemplate = htmlTemplate.Replace("{{ClientName}}", sale.Customer?.Name ?? "MOSTRADOR");
            htmlTemplate = htmlTemplate.Replace("{{SubTotal}}", subTotal);
            htmlTemplate = htmlTemplate.Replace("{{totalTaxes}}", totalTaxes);
            htmlTemplate = htmlTemplate.Replace("{{Discount}}", ToMoneyString(sale.Discount));
            htmlTemplate = htmlTemplate.Replace("{{GrandTotal}}", GetTotalAmount(sale));
            htmlTemplate = htmlTemplate.Replace("{{CreationDate}}", DateTime.Now.ToString("dd/MM/yyyy"));
            htmlTemplate = htmlTemplate.Replace("{{CompanyName}}", company?.Name ?? "Factura");
            htmlTemplate = htmlTemplate.Replace("{{CompanySlogan}}", company?.Slogan ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{CompanyAddress}}", company?.Address ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{CompanyPhone}}", company?.ContactPhone ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{User}}", sale.UserName);

            string payCondition = sale.CreditSale == true ? "CRÉDITO" : "CONTADO";
            htmlTemplate = htmlTemplate.Replace("{{PaymentConditions}}", sale.IsAQuotation ? "" :
                $"<p class=\"ClientP\"><span style=\"font-weight: bold; font-size: 18px\">Condiciones de pago: </span>{payCondition}</p>");

            return htmlTemplate;
        }

        public static string GenerateFinalReceiptHtml(AddEditSale sale, string htmlTemplate, CompanyViewModel? company, bool duplicate = false)
        {
            string productsHTML = string.Empty;
            string totalTaxes = ToMoneyString(sale.SaleProducts!.Sum(p => p.ITBIS));
            string subTotal = ToMoneyString(sale.SaleProducts!.Sum(p => p.Total) - sale.SaleProducts!.Sum(p => p.ITBIS));
            List<string> changesText = new List<string>();//!string.IsNullOrEmpty(sale.originalSaleCode) ? "Modificada" : string.Empty;
            if(!string.IsNullOrEmpty(sale.originalSaleCode))
                changesText.Add("Modificada");
            if (duplicate)
                changesText.Add("Reimpresion");

            foreach (var product in sale.SaleProducts!)
            {
                productsHTML += $"<tr><td colspan=\"3\">{product.ProductDescription}</td></tr>" +
                                $"<tr><td>{product.Quantity}</td>" +
                                $"<td class=\"price\">{ToMoneyString(product.UnitPrice)}</td>" +
                                $"<td class=\"price\">{ToMoneyString(product.Total)}</td></tr>";
            }

            if (sale.WorkForceValue != null && sale.WorkForceValue > 0)
            {
                productsHTML += $"<tr><td colspan=\"3\">Mano de obra</td></tr>" +
                                $"<tr><td>000</td>" +
                                $"<td class=\"price\">000</td>" +
                                $"<td class=\"price\">{ToMoneyString(sale.WorkForceValue)}</td></tr>";
            }

            htmlTemplate = htmlTemplate.Replace("{{SaleTitle}}", sale.IsAQuotation ? "COTIZACIÓN" : "FACTURA");
            htmlTemplate = htmlTemplate.Replace("{{SaleCode}}", sale.Code);
            htmlTemplate = htmlTemplate.Replace("{{CompanyName}}", company?.Name ?? "Factura");
            htmlTemplate = htmlTemplate.Replace("{{CompanySlogan}}", company?.Slogan ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{CompanyAddress}}", company?.Address ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{CompanyPhone}}", company?.ContactPhone ?? string.Empty);
            htmlTemplate = htmlTemplate.Replace("{{ClientName}}", (sale.Customer?.Name ?? "MOSTRADOR"));
            htmlTemplate = htmlTemplate.Replace("{{SubTotal}}", subTotal);
            htmlTemplate = htmlTemplate.Replace("{{totalTaxes}}", totalTaxes);
            htmlTemplate = htmlTemplate.Replace("{{Discount}}", ToMoneyString(sale.Discount));
            htmlTemplate = htmlTemplate.Replace("{{GrandTotal}}", GetTotalAmount(sale));
            htmlTemplate = htmlTemplate.Replace("{{Products}}", productsHTML);
            htmlTemplate = htmlTemplate.Replace("{{CreationDate}}", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            htmlTemplate = htmlTemplate.Replace("{{PaymentType}}", sale.GetPaymentType());
            htmlTemplate = htmlTemplate.Replace("{{User}}", sale.UserName);
            htmlTemplate = htmlTemplate.Replace("{{DuplicateText}}", changesText.Any() ? string.Join(" - ", changesText) : "");

            string payCondition = sale.CreditSale == true ? "CRÉDITO" : "CONTADO";
            htmlTemplate = htmlTemplate.Replace("{{PaymentConditions}}", sale.IsAQuotation ? "" :
                $"Condiciones de pago: <span>{payCondition}</span>");

            return htmlTemplate;
        }

        private static string GetPaymentType(this AddEditSale sale)
        {
            if(sale.TCAmount != null && sale.TCAmount > 0)
            {
                return "Tarjeta de crédito";
            }
            if (sale.TCAmount != null && sale.TCAmount > 0)
            {
                return "Transferencia";
            }

            return "Efectivo";
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

        private static string ReplaceRange(string input, string startMarker, string endMarker, string replacement)
        {
            int startIndex = input.IndexOf(startMarker);
            int endIndex = input.IndexOf(endMarker, startIndex + startMarker.Length);

            if (startIndex != -1 && endIndex != -1)
            {
                // Include the markers in the replacement
                return input.Substring(0, startIndex) + replacement + input.Substring(endIndex + endMarker.Length);
            }

            return input; // Return original if markers are not found
        }

        private static string ExtractBetweenMarkers(string input, string startMarker, string endMarker)
        {
            int startIndex = input.IndexOf(startMarker);
            int endIndex = input.IndexOf(endMarker, startIndex + startMarker.Length);

            if (startIndex != -1 && endIndex != -1)
            {
                // Move startIndex forward to the content inside markers
                startIndex += startMarker.Length;
                return input.Substring(startIndex, endIndex - startIndex);
            }

            return string.Empty; // Return empty if markers are not found
        }
    }
}
