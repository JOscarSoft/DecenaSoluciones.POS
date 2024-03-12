using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface ICompanyService
    {
        Task<ApiResponse<List<CompanyViewModel>>> GetCompanyList();
        Task<ApiResponse<CompanyViewModel>> AddNewCompany(AddEditCompany company);
        Task<ApiResponse<CompanyViewModel>> UpdateCompany(int id, AddEditCompany company);
        Task<ApiResponse<bool>> RemoveCompany(int id);
    }
}
