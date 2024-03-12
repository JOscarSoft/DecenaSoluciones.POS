using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface ICompanyService
    {
        Task<ApiResponse<List<CompanyViewModel>>> GetCompanyList();
        Task<ApiResponse<CompanyViewModel>> GetCompany(int companyId);
        Task<ApiResponse<CompanyViewModel>> AddNewCompany(AddEditCompany company);
        Task<ApiResponse<CompanyViewModel>> UpdateCompany(int id, AddEditCompany company);
        Task<ApiResponse<int>> RemoveCompany(int id);
    }
}
