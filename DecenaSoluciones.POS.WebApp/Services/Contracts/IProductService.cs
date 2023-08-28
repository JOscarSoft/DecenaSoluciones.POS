using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.WebApp.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductViewModel>>> GetProducts();
        Task<ApiResponse<ProductViewModel>> GetProductByCode(string code);
        Task<ApiResponse<ProductViewModel>> GetProductById(int id);
        Task<ApiResponse<int>> AddNewProduct(AddEditProduct product);
        Task<ApiResponse<ProductViewModel>> UpdateProduct(int id, AddEditProduct product);
        Task<ApiResponse<int>> RemoveProduct(int id);
    }
}
