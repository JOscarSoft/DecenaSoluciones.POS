using DecenaSoluciones.POS.Shared.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DecenaSoluciones.POS.WebApp.Services
{
    public class SaleService : ISaleService
    {
        private readonly HttpClient _httpClient;
        public SaleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<AddEditCustomer>> UpdateCustomer(int id, AddEditCustomer customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Customer/{id}", customer);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditCustomer>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<List<AddEditSale>>> GetSalesList()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<AddEditSale>>>("api/Sale");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetSaleById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<AddEditSale>>($"api/Sale/{id}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetSaleByCode(string code)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<AddEditSale>>($"api/Sale/GetByCode/{code}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<List<AddEditSale>>> GetQuotationsList()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<AddEditSale>>>("api/Sale/quotation");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetQuotationById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<AddEditSale>>($"api/Sale/quotation/{id}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> GetQuotationByCode(string code)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<AddEditSale>>($"api/Sale/quotation/GetByCode/{code}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

            return result;
        }

        public async Task<ApiResponse<AddEditSale>> AddNewSale(AddEditSale sale)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Sale", sale);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<AddEditSale>> UpdateSale(int id, AddEditSale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Sale/{id}", sale);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditSale>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Ventas.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }
    }
}
