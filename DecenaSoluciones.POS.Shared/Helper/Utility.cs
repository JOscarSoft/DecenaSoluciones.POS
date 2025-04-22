using DecenaSoluciones.POS.Shared.Dtos;
using System.Globalization;

namespace DecenaSoluciones.POS.Shared.Helper
{
    public static class Utility
    {
        private const string StartProductsIndicator = "<!--StartProductLine-->";
        private const string EndProductsIndicator = "<!--EndProductLine-->";
        public static string GenerateReceiptHtml(AddEditSale sale, string htmlTemplate, CompanyViewModel? company, bool duplicate = false)
        {
            string productsHTML = string.Empty;
            string totalTaxes = ToMoneyString(sale.SaleProducts!.Sum(p => p.ITBIS));
            string subTotal = ToMoneyString(sale.SaleProducts!.Sum(p => p.Total) - sale.SaleProducts!.Sum(p => p.ITBIS));
            List<string> changesText = [];
            if (!string.IsNullOrEmpty(sale.originalSaleCode))
                changesText.Add("Modificada");
            if (duplicate)
                changesText.Add("Reimpresion");

            int count = 1;

            string trProducts = ExtractBetweenMarkers(htmlTemplate, StartProductsIndicator, EndProductsIndicator);
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
                count++;
            }

            if (sale.WorkForceValue != null && sale.WorkForceValue > 0)
            {

                string trItem = trProducts
                    .Replace("{{Product.Index}}", count.ToString())
                    .Replace("{{Product.Code}}", "000")
                    .Replace("{{Product.Description}}", "Mano de Obra")
                    .Replace("{{Product.Quantity}}", "1")
                    .Replace("{{Product.Price}}", ToMoneyString(sale.WorkForceValue))
                    .Replace("{{Product.ITBIS}}", "-")
                    .Replace("{{Product.Amount}}", ToMoneyString(sale.WorkForceValue));


                productsHTML += trItem;
            }

            htmlTemplate = ReplaceRange(htmlTemplate, StartProductsIndicator, EndProductsIndicator, productsHTML)
                .Replace("{{SaleTitle}}", sale.IsAQuotation ? "COTIZACIÓN" : "FACTURA")
                .Replace("{{SaleCode}}", sale.Code)
                .Replace("{{ClientName}}", sale.Customer?.Name ?? "MOSTRADOR")
                .Replace("{{SubTotal}}", subTotal)
                .Replace("{{totalTaxes}}", totalTaxes)
                .Replace("{{Discount}}", ToMoneyString(sale.Discount))
                .Replace("{{GrandTotal}}", GetTotalAmount(sale))
                .Replace("{{CreationDate}}", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"))
                .Replace("{{CompanyName}}", company?.Name ?? "Factura")
                .Replace("{{CompanySlogan}}", company?.Slogan ?? string.Empty)
                .Replace("{{CompanyAddress}}", company?.Address ?? string.Empty)
                .Replace("{{CompanyPhone}}", company?.ContactPhone ?? string.Empty)
                .Replace("{{User}}", sale.UserName)
                .Replace("{{PaymentType}}", sale.GetPaymentType())
                .Replace("{{PaymentConditions}}", sale.CreditSale == true ? "CRÉDITO" : "CONTADO")
                .Replace("{{DuplicateText}}", changesText.Any() ? string.Join(" - ", changesText) : "");

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
