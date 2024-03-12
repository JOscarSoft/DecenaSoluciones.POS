using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetCompanyList();
        Task<CompanyViewModel> AddNewCompany(AddEditCompany company);
        Task<CompanyViewModel> UpdateCompany(int id, AddEditCompany company);
        Task<int> RemoveCompany(int id);
    }
}
