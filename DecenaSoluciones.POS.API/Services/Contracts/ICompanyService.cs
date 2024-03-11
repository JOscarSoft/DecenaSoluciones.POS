using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetCompanyList();
        Task<CompanyViewModel> AddNewCompany(AddEditCompany product);
        Task<CompanyViewModel> UpdateCompany(int id, AddEditCompany product);
        Task<int> RemoveCompany(int id);
    }
}
