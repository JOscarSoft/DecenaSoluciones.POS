using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<ApiResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Authentication/setpassword", model);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<List<RegistrationViewModel>>> GetUsersList()
        {
            var response = await _httpClient.GetAsync("api/Authentication/users");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<RegistrationViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<List<RegistrationViewModel>>> GetUsersList(int companyid)
        {
            var response = await _httpClient.GetAsync($"api/Authentication/users/company/{companyid}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<RegistrationViewModel>>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<string>> Login(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Authentication/login", model);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<bool>> Registration(RegistrationViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Authentication/newuser", model);

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<bool>> RemoveUser(string userName)
        {
            var response = await _httpClient.DeleteAsync($"api/Authentication/removeuser/{userName}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }
    }
}
