using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IQuotationService
    {
        Task<List<AddEditSale>> GetQuotationsList();
        Task<AddEditSale> GetQuotationById(int id);
        Task<AddEditSale> GetQuotationByCode(string code);
        Task<AddEditSale> AddNewQuotation(Quotation Quotation);
        Task<AddEditSale> UpdateQuotation(int id, Quotation Quotation);
    }
}
