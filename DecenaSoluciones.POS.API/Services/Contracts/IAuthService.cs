using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IAuthService
    {
        Task<bool> Registration(RegistrationViewModel model);
        Task<string> Login(LoginViewModel model);
        Task<bool> ChangePassword(ChangePasswordViewModel model);
        Task<bool> RemoveUser(string userName);
        Task<List<RegistrationViewModel>> GetUsersList();
        Task<List<RegistrationViewModel>> GetCompanyUsersList(int CompanyId);
    }
}
