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
            var customers = await _dbContext.Customers
                .Include(p => p.CustomerProducts)
                .ThenInclude(p => p.Product)
                .ToListAsync();
            return _mapper.Map<List<CustomerViewModel>>(customers);
        }

        public async Task<AddEditCustomer> GetCustomerById(int id)
        {
            var customer = await _dbContext.Customers
                .Include(p => p.CustomerProducts)
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditCustomer>(customer);
        }

        public async Task<AddEditCustomer> AddNewCustomer(AddEditCustomer customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
            foreach (var product in newCustomer.CustomerProducts)
                product.Product = null;

            _dbContext.Customers.Add(newCustomer);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditCustomer>(newCustomer);
        }

        public async Task<AddEditCustomer> UpdateCustomer(int id, AddEditCustomer customer)
        {
            var oldCustomer = await _dbContext.Customers
                .Include(p => p.CustomerProducts)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el cliente a editar.");

            var newCustomer = _mapper.Map<Customer>(customer);
            foreach(var product in newCustomer.CustomerProducts)
                product.Product = null;

            _dbContext.Customers.Update(newCustomer);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();

            var Ids = oldCustomer.CustomerProducts!
                .Where(p => !newCustomer.CustomerProducts.Any(n => n.Id == p.Id))
                .Select(p => p.Id);

            foreach(var custProdId in Ids)
            {
                _dbContext.CustomerProducts.Remove(oldCustomer.CustomerProducts.FirstOrDefault(p=>p.Id == custProdId));
            }
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
