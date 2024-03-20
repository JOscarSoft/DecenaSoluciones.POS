using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetCustomerList();
        Task<AddEditCustomer> GetCustomerById(int id);
        Task<AddEditCustomer> AddNewCustomer(AddEditCustomer customer);
        Task<AddEditCustomer> UpdateCustomer(int id, AddEditCustomer customer);
        Task<bool> RemoveCustomer(int id);
        Task AddProductToCustomer(int productId, int customerId, int quantity);
    }
}
