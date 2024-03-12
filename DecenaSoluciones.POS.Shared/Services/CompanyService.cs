using DecenaSoluciones.POS.Shared.Dtos;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;
        public CompanyService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<List<CompanyViewModel>>> GetCompanyList()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<CompanyViewModel>>>("api/Company");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Compañías.");

            return result;
        }

        public async Task<ApiResponse<CompanyViewModel>> AddNewCompany(AddEditCompany company)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Company", company);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CompanyViewModel>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Compañías.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<CompanyViewModel>> UpdateCompany(int id, AddEditCompany company)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Company/{id}", company);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<CompanyViewModel>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Compañías.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<int>> RemoveCompany(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Company/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de Compañías.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<CompanyViewModel>> GetCompany(int companyId)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<CompanyViewModel>>($"api/Company/{companyId}");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Compañías.");

            return result;
        }
    }
}
