using DecenaSoluciones.POS.Shared.Dtos;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<ProductViewModel>> AddNewProduct(AddEditProduct product)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product", product);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de productos.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }
        
        public async Task<ApiResponse<ProductViewModel>> GetProductByCode(string code)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<ProductViewModel>>($"api/Product/GetByCode/{code}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<ProductViewModel>> GetProductById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<ProductViewModel>>($"api/Product/{id}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<List<ProductViewModel>>> GetProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProductViewModel>>>("api/Product");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<List<ProductViewModel>>> GetAssinablesProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProductViewModel>>>("api/Product/GetAssignables");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<int>> RemoveProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de productos.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<ProductViewModel>> UpdateProduct(int id, AddEditProduct product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Product/{id}", product);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de productos.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<bool>> UpdateInventary(List<UpdateInventory> inventoryItems)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/UpdateInventary", inventoryItems);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de productos.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }
    }
}
