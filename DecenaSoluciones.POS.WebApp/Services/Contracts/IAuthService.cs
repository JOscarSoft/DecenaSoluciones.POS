using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.WebApp.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<bool>> Registration(RegistrationViewModel model);
        Task<ApiResponse<string>> Login(LoginViewModel model);
        Task<ApiResponse<bool>> ChangePassword(ChangePasswordViewModel model);
        Task<ApiResponse<bool>> RemoveUser(string userName);
        Task<ApiResponse<List<RegistrationViewModel>>> GetUsersList();
    }
}
