using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using DecenaSoluciones.POS.Shared.Helper;
using System.Globalization;
using System.Net.Http.Json;
using System.Xml.Linq;

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

        public async Task<Stream> AddNewMobileSale(AddEditSale sale)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Sale/mobilesale", sale);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadAsStreamAsync();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de reportes.");

            return result;
        }

        public async Task<Stream> UpdateMobileSale(int id, AddEditSale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Sale/mobilesale/{id}", sale);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadAsStreamAsync();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de reportes.");

            return result;
        }

        private async Task<string> GenerateReceipt(AddEditSale sale)
        {
            try
            {
                var companyId = (await _localStorage.GetStorage<UserInfoExtension>("userSession"))!.CompanyId;

                string htmlTemplate = string.IsNullOrWhiteSpace(companyId) || int.Parse(companyId) == 1 ?
                    await _httpClientLocal.GetStringAsync("templates/DecenaReceiptHTML.HTML") :
                    await _httpClientLocal.GetStringAsync("templates/ReceiptHTML.HTML");

                return Utility.GenerateReceiptHtml(sale, htmlTemplate);
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
