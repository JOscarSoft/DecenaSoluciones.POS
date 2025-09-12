using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
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

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<ProductViewModel>> GetProductByCode(string code)
        {
            var response = await _httpClient.GetAsync($"api/Product/GetByCode/{code}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Productos.");

            return result;
        }

        public async Task<ApiResponse<LastSaleXProductViewModel>> GetLastSaleXProduct(int productId)
        {
            var response = await _httpClient.GetAsync($"api/Product/GetLastSale/{productId}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LastSaleXProductViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Productos.");

            return result;
        }

        public async Task<ApiResponse<ProductViewModel>> GetProductById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Product/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Productos.");

            return result;
        }

        public async Task<ApiResponse<List<ProductViewModel>>> GetProducts()
        {
            var response = await _httpClient.GetAsync("api/Product");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Productos.");

            return result;
        }

        public async Task<ApiResponse<List<ProductViewModel>>> GetAssinablesProducts()
        {
            var response = await _httpClient.GetAsync("api/Product/GetAssignables");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Productos.");

            return result;
        }

        public async Task<ApiResponse<int>> RemoveProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Product/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }

        public async Task<ApiResponse<ProductViewModel>> UpdateProduct(int id, AddEditProduct product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Product/{id}", product);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de productos.");

            return result;
        }
    }
}
