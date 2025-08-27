using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface IProviderService
    {
        Task<ApiResponse<List<AddEditProvider>>> GetProviderList();
        Task<ApiResponse<AddEditProvider>> GetProviderById(int id);
        Task<ApiResponse<AddEditProvider>> AddNewProvider(AddEditProvider Provider);
        Task<ApiResponse<AddEditProvider>> UpdateProvider(int id, AddEditProvider Provider);

        Task<ApiResponse<bool>> RemoveProvider(int id);
    }
}
