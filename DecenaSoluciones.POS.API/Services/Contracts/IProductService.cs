using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProductList();
        Task<List<ProductViewModel>> GetAssignables();
        Task<ProductViewModel> GetProductById(int id);
        Task<ProductViewModel> GetProductByCode(string code);
        Task<ProductViewModel> AddNewProduct(AddEditProduct product);
        Task<ProductViewModel> UpdateProduct(int id, AddEditProduct product);
        Task<bool> UpdateInventary(List<UpdateInventory> inventoryItems);
        Task<int> RemoveProduct(int id);
        Task<ProductViewModel> UpdateProductStock(int id, int quantity);
        Task<LastSaleXProductViewModel> GetLastSaleXProduct(int id);
    }
}
