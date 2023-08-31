using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface ISaleService
    {
        Task<List<AddEditSale>> GetSalesList();
        Task<AddEditSale> GetSaleById(int id);
        Task<AddEditSale> GetSaleByCode(string code);
        Task<AddEditSale> AddNewSale(Sale sale);
        Task<AddEditSale> UpdateSale(int id, Sale sale);
    }
}
