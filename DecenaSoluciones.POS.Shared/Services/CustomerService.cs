using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        public CustomerService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<List<CustomerViewModel>>> GetCustomerList()
        {
            var response = await _httpClient.GetAsync("api/Customer");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CustomerViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<AddEditCustomer>> GetCustomerById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Customer/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditCustomer>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<AddEditCustomer>> AddNewCustomer(AddEditCustomer customer)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Customer", customer);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditCustomer>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<AddEditCustomer>> UpdateCustomer(int id, AddEditCustomer customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Customer/{id}", customer);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditCustomer>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<bool>> RemoveCustomer(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Customer/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de clientes.");

            return result;
        }
    }
}
