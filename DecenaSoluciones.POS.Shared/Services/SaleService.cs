using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Globalization;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class SaleService : ISaleService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientLocal;
        private readonly ILocalStorage _localStorage;

        public SaleService(IHttpClientFactory clientFactory, ILocalStorage localStorage)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
            _httpClientLocal = clientFactory.CreateClient("Local");
            _localStorage = localStorage;
        }

        public async Task<ApiResponse<List<SalesViewModel>>> GetSalesList()
        {
            var response = await _httpClient.GetAsync("api/Sale");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<SalesViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetSaleById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Sale/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetSaleByCode(string code)
        {
            var response = await _httpClient.GetAsync($"api/Sale/GetByCode/{code}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<List<SalesViewModel>>> GetQuotationsList()
        {
            var response = await _httpClient.GetAsync("api/Sale/quotation");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<SalesViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetQuotationById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Sale/quotation/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetQuotationByCode(string code)
        {
            var response = await _httpClient.GetAsync($"api/Sale/quotation/GetByCode/{code}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<(string, ApiResponse<AddEditSale>)> AddNewSale(AddEditSale sale)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Sale", sale);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            sale.Code = result.Result!.Code;
            var receipt = await GenerateReceipt(sale);
            return (receipt, result);
        }

        public async Task<(string, ApiResponse<AddEditSale>)> UpdateSale(int id, AddEditSale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Sale/{id}", sale);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            var receipt = await GenerateReceipt(sale);
            return (receipt, result);
        }

        private async Task<string> GenerateReceipt(AddEditSale sale)
        {
            try
            {
                var companyId = (await _localStorage.GetStorage<UserInfoExtension>("userSession"))!.CompanyId;

                string htmlTemplate = string.IsNullOrWhiteSpace(companyId) || int.Parse(companyId) == 1 ?
                    await _httpClientLocal.GetStringAsync("templates/DecenaReceiptHTML.HTML") :
                    await _httpClientLocal.GetStringAsync("templates/ReceiptHTML.HTML");

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
            catch
            {
                return string.Empty;
            }
        }
        private string GetPageBreak() => "</table><p style=\"page-break-before: always\">" +
                            "<table class=\"tblProducts\">" +
                            "<tr><th>ITEMS</th>" +
                            "<th>PRODUCTO</th>" +
                            "<th>CANT.</th>" +
                            "<th>PRECIO</th>" +
                            "<th>ITBIS</th>" +
                            "<th>TOTAL</th></tr>";

        private string GetTotalAmount(AddEditSale sale)
        {
            decimal calc = 0.0M;
            calc += sale.SaleProducts!.Sum(p => p.Total);
            calc += sale.WorkForceValue ?? 0.0M;
            calc -= sale.Discount ?? 0.0M;
            return ToMoneyString(calc);
        }

        private string ToMoneyString(decimal? value) => value.HasValue ? $"{value.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"))}" : "0.00";
    }
}
