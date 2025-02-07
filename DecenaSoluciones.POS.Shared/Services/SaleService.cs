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
        private readonly ICompanyService _companyService;

        public SaleService(IHttpClientFactory clientFactory, ILocalStorage localStorage, ICompanyService companyService)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
            _httpClientLocal = clientFactory.CreateClient("Local");
            _localStorage = localStorage;
            _companyService = companyService;
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

        public async Task<ApiResponse<string>> ReGenerateReceipt(string saleCode)
        {
            var response = await _httpClient.GetAsync($"api/Sale/GetByCode/{saleCode}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            var receipt = await GenerateReceipt(result.Result!, true);

            return new ApiResponse<string> { Success = true, Result = receipt };
        }

        public async Task<(string, ApiResponse<AddEditSale>)> UpdateSale(int id, AddEditSale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Sale/{id}", sale);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            var receipt = await GenerateReceipt(result.Result);
            return (receipt, result);
        }

        public async Task<ApiResponse<AddEditSale>> DismissSale(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/Sale/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
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

        public async Task<string> GetDefaultSaleTemplateAsync()
        {
            try
            {                
              return await _httpClientLocal.GetStringAsync("templates/SaleReceiptHTML.HTML");
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<string> GetDefaultQuotationTemplateAsync(int companyId)
        {
            try
            {
                return await _httpClientLocal.GetStringAsync(companyId == 1 ? "templates/DecenaQuotationReceiptHTML.HTML" : "templates/QuotationReceiptHTML.HTML");
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<string> GenerateReceipt(AddEditSale sale, bool duplicate = false)
        {
            try
            {
                CompanyViewModel? company = new CompanyViewModel();   
                var companyId = (await _localStorage.GetStorage<UserInfoExtension>("userSession"))!.CompanyId;

                if(companyId != null)
                    company = (await _companyService.GetCompany(int.Parse(companyId)))?.Result;

                string? htmlTemplate = sale.IsAQuotation ? company?.QuotationsReceipt : company?.SalesReceipt;

                if (string.IsNullOrEmpty(htmlTemplate))
                {
                    htmlTemplate = 
                        await _httpClientLocal.GetStringAsync(sale.IsAQuotation ?  "templates/QuotationReceiptHTML.HTML" : "templates/SaleReceiptHTML.HTML");
                }


                return sale.IsAQuotation ? Utility.GenerateQuotationReceiptHtml(sale, htmlTemplate, company?.ContactName ?? "") : Utility.GenerateFinalReceiptHtml(sale, htmlTemplate, company, duplicate);
            }
            catch(Exception ex) 
            {
                return string.Empty;
            }
        }
    }
}
