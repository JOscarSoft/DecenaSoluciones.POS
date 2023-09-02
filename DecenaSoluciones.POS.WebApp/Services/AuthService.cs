using DecenaSoluciones.POS.Shared.Dtos;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.WebApp.Services
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

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<List<RegistrationViewModel>>> GetUsersList()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<RegistrationViewModel>>>("api/Authentication/users");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

            return result;
        }

        public async Task<ApiResponse<string>> Login(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Authentication/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<bool>> Registration(RegistrationViewModel model)
        {            
            var response = await _httpClient.PostAsJsonAsync($"api/Authentication/newuser", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<bool>> RemoveUser(string userName)
        {
            var response = await _httpClient.DeleteAsync($"api/Authentication/removeuser/{userName}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de autenticación.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }
    }
}
