using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class MiscellaneousExpenseService : IMiscellaneousExpenseService
    {
        private readonly HttpClient _httpClient;
        public MiscellaneousExpenseService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<List<AddEditMiscellaneousExpense>>> GetMiscellaneousExpenseList()
        {
            var response = await _httpClient.GetAsync("api/MiscellaneousExpense");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AddEditMiscellaneousExpense>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de gastos.");

            return result;
        }

        public async Task<ApiResponse<AddEditMiscellaneousExpense>> GetMiscellaneousExpenseById(int id)
        {
            var response = await _httpClient.GetAsync($"api/MiscellaneousExpense/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditMiscellaneousExpense>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de gastos.");

            return result;
        }

        public async Task<ApiResponse<AddEditMiscellaneousExpense>> AddNewMiscellaneousExpense(AddEditMiscellaneousExpense MiscellaneousExpense)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/MiscellaneousExpense", MiscellaneousExpense);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditMiscellaneousExpense>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de gastos.");

            return result;
        }

        public async Task<ApiResponse<AddEditMiscellaneousExpense>> UpdateMiscellaneousExpense(int id, AddEditMiscellaneousExpense MiscellaneousExpense)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/MiscellaneousExpense/{id}", MiscellaneousExpense);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AddEditMiscellaneousExpense>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de gastos.");

            return result;
        }

        public async Task<ApiResponse<bool>> RemoveMiscellaneousExpense(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/MiscellaneousExpense/{id}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de gastos.");

            return result;
        }
    }
}
