using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IProviderService
    {
        Task<List<AddEditProvider>> GetProvidersList();
        Task<AddEditProvider> GetProviderById(int id);
        Task<AddEditProvider> AddNewProvider(AddEditProvider provider);
        Task<AddEditProvider> UpdateProvider(int id, AddEditProvider provider);
        Task<bool> RemoveProvider(int id);
    }
}
