using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface IInventoryService
    {
        Task<ApiResponse<List<InventoryEntryViewModel>>> GetInventoryEntryList();
        Task<ApiResponse<InventoryEntryViewModel>> GetInventoryEntryById(int id);
        Task<ApiResponse<InventoryEntryViewModel>> AddNewInventoryEntry(InventoryEntryViewModel inventoryEntry);
        Task<ApiResponse<InventoryEntryViewModel>> UpdateInventoryEntry(int id, InventoryEntryViewModel inventoryEntry);

        Task<ApiResponse<bool>> RemoveInventoryEntry(int id);
    }
}
