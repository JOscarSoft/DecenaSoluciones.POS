using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.WebApp.Services
{
    public interface ISaleService
    {
        Task<ApiResponse<List<AddEditSale>>> GetSalesList();
        Task<ApiResponse<AddEditSale>> GetSaleById(int id);
        Task<ApiResponse<AddEditSale>> GetSaleByCode(string code);
        Task<ApiResponse<List<AddEditSale>>> GetQuotationsList();
        Task<ApiResponse<AddEditSale>> GetQuotationById(int id);
        Task<ApiResponse<AddEditSale>> GetQuotationByCode(string code);
        Task<ApiResponse<AddEditSale>> AddNewSale(AddEditSale sale);
        Task<ApiResponse<AddEditSale>> UpdateSale(int id, AddEditSale sale);
    }
}
