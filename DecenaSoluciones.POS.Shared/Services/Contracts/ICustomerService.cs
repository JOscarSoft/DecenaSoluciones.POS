using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<List<CustomerViewModel>>> GetCustomerList();
        Task<ApiResponse<AddEditCustomer>> GetCustomerById(int id);
        Task<ApiResponse<AddEditCustomer>> AddNewCustomer(AddEditCustomer customer);
        Task<ApiResponse<AddEditCustomer>> UpdateCustomer(int id, AddEditCustomer customer);
        Task<ApiResponse<bool>> RemoveCustomer(int id);
    }
}
