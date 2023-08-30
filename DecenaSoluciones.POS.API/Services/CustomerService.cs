using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public CustomerService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CustomerViewModel>> GetCustomerList()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            return _mapper.Map<List<CustomerViewModel>>(customers);
        }

        public async Task<AddEditCustomer> GetCustomerById(int id)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditCustomer>(customer);
        }

        public async Task<AddEditCustomer> AddNewCustomer(AddEditCustomer customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
            _dbContext.Customers.Add(newCustomer);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditCustomer>(newCustomer);
        }

        public async Task<AddEditCustomer> UpdateCustomer(int id, AddEditCustomer customer)
        {
            var oldCustomer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el cliente a editar.");

            var newCustomer = _mapper.Map<Customer>(customer);
            newCustomer.CustomerQuotations = oldCustomer.CustomerQuotations;
            newCustomer.CustomerSales = oldCustomer.CustomerSales;
            _dbContext.Customers.Update(newCustomer);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditCustomer>(newCustomer);
        }

        public async Task<bool> RemoveCustomer(int id)
        {
            var existProducts = await _dbContext.CustomerProducts.AnyAsync(p => p.CustomerId == id);
            var existQuotation = await _dbContext.Quotation.AnyAsync(p => p.CustomerId == id);
            var existSales = await _dbContext.Sale.AnyAsync(p => p.CustomerId == id);
            if (existProducts || existQuotation || existSales)
                throw new Exception("El producto no puede ser eliminado porque existen registros asociados a el.");

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el cliente a eliminar.");

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
