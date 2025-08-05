using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class ProviderService : IProviderService
    {
        private readonly HttpClient _httpClient;
        public ProviderService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<List<AddEditProvider>>> GetProviderList()
        {
            var response = await _httpClient.GetAsync("api/Provider");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AddEditProvider>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de proveedores.");

            return result;
        }

        public async Task<ApiResponse<AddEditProvider>> GetProviderById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Provider/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditProvider>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de proveedores.");

            return result;
        }

        public async Task<ApiResponse<AddEditProvider>> AddNewProvider(AddEditProvider Provider)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Provider", Provider);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditProvider>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de proveedores.");

            return result;
        }

        public async Task<ApiResponse<AddEditProvider>> UpdateProvider(int id, AddEditProvider Provider)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Provider/{id}", Provider);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditProvider>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de proveedores.");

            return result;
        }

        public async Task<ApiResponse<bool>> RemoveProvider(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Provider/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de proveedores.");

            return result;
        }
    }
}
