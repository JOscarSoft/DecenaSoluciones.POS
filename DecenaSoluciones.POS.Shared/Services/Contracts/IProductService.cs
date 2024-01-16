using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductViewModel>>> GetProducts();
        Task<ApiResponse<List<ProductViewModel>>> GetAssinablesProducts();
        Task<ApiResponse<ProductViewModel>> GetProductByCode(string code);
        Task<ApiResponse<ProductViewModel>> GetProductById(int id);
        Task<ApiResponse<ProductViewModel>> AddNewProduct(AddEditProduct product);
        Task<ApiResponse<ProductViewModel>> UpdateProduct(int id, AddEditProduct product);
        Task<ApiResponse<int>> RemoveProduct(int id);
        Task<ApiResponse<bool>> UpdateInventary(List<UpdateInventory> inventoryItems);
    }
}
