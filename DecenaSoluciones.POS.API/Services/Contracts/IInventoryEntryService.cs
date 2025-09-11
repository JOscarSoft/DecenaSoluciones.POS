using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IInventoryEntryService
    {
        Task<List<InventoryEntryViewModel>> GetInventoryEntryList();
        Task<InventoryEntryViewModel> GetInventoryEntryById(int id);
        Task<InventoryEntryViewModel> AddNewInventoryEntry(InventoryEntry inventoryEntry);
        Task<InventoryEntryViewModel> UpdateInventoryEntry(int id, InventoryEntry inventoryEntry);
        Task<bool> RemoveInventoryEntry(int id);
        Task AddNewInventoryEntryFromProductChange(Product productModification, decimal difference);

    }
}
