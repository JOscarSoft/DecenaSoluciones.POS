using DecenaSoluciones.POS.Shared.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DecenaSoluciones.POS.WebApp.Services
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
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<CustomerViewModel>>>("api/Customer");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<AddEditCustomer>> GetCustomerById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<AddEditCustomer>>($"api/Customer/{id}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Clientes.");

            return result;
        }

        public async Task<ApiResponse<AddEditCustomer>> AddNewCustomer(AddEditCustomer customer)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Customer", customer);

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

        public async Task<ApiResponse<bool>> RemoveCustomer(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Customer/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de clientes.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }
    }
}
