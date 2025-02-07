using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface ISaleService
    {
        Task<ApiResponse<List<SalesViewModel>>> GetSalesList();
        Task<ApiResponse<AddEditSale>> GetSaleById(int id);
        Task<ApiResponse<AddEditSale>> GetSaleByCode(string code);
        Task<ApiResponse<List<SalesViewModel>>> GetQuotationsList();
        Task<ApiResponse<AddEditSale>> GetQuotationById(int id);
        Task<ApiResponse<AddEditSale>> GetQuotationByCode(string code);
        Task<(string, ApiResponse<AddEditSale>)> AddNewSale(AddEditSale sale);
        Task<(string, ApiResponse<AddEditSale>)> UpdateSale(int id, AddEditSale sale);
        Task<ApiResponse<AddEditSale>> DismissSale(int id);
        Task<Stream> AddNewMobileSale(AddEditSale sale);
        Task<Stream> UpdateMobileSale(int id, AddEditSale sale);
        Task<ApiResponse<string>> ReGenerateReceipt(string saleCode);
        Task<string> GetDefaultSaleTemplateAsync();
        Task<string> GetDefaultQuotationTemplateAsync(int companyId);
    }
}
