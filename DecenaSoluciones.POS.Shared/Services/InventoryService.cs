using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        public InventoryService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<List<InventoryEntryViewModel>>> GetInventoryEntryList()
        {
            var response = await _httpClient.GetAsync("api/InventoryEntry");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<InventoryEntryViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de inventario.");

            return result;
        }

        public async Task<ApiResponse<InventoryEntryViewModel>> GetInventoryEntryById(int id)
        {
            var response = await _httpClient.GetAsync($"api/InventoryEntry/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<InventoryEntryViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de inventario.");

            return result;
        }

        public async Task<ApiResponse<InventoryEntryViewModel>> AddNewInventoryEntry(InventoryEntryViewModel inventoryEntry)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/InventoryEntry", inventoryEntry);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<InventoryEntryViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de inventario.");

            return result;
        }

        public async Task<ApiResponse<InventoryEntryViewModel>> UpdateInventoryEntry(int id, InventoryEntryViewModel inventoryEntry)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/InventoryEntry/{id}", inventoryEntry);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<InventoryEntryViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de inventario.");

            var updatedEntry = result.Result ?? new InventoryEntryViewModel();

            foreach (var item in updatedEntry.Details!)
            {
                var product = inventoryEntry.Details!.FirstOrDefault(p => p.ProductId == item.ProductId);
                item.ProductCode = product?.ProductCode ?? string.Empty;
                item.ProductDescription = product?.ProductDescription ?? string.Empty;
            }

            return result;
        }

        public async Task<ApiResponse<bool>> RemoveInventoryEntry(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/InventoryEntry/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de inventario.");

            return result;
        }
    }
}
