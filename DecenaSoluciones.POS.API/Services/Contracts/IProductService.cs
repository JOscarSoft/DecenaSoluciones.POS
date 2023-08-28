using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProductList();
        Task<ProductViewModel> GetProductById(int id);
        Task<ProductViewModel> GetProductByCode(string code);
        Task<int> AddNewProduct(AddEditProduct product);
        Task<ProductViewModel> UpdateProduct(int id, AddEditProduct product);
        Task<int> RemoveProduct(int id);
    }
}
