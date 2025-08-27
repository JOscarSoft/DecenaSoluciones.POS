using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class ProviderService : IProviderService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public ProviderService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AddEditProvider>> GetProvidersList()
        {
            var providers = await _dbContext.Providers
                .ToListAsync();
            return _mapper.Map<List<AddEditProvider>>(providers);
        }

        public async Task<AddEditProvider> GetProviderById(int id)
        {
            var provider = await _dbContext.Providers
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditProvider>(provider);
        }

        public async Task<AddEditProvider> AddNewProvider(AddEditProvider provider)
        {
            var newProvider = _mapper.Map<Provider>(provider);

            _dbContext.Providers.Add(newProvider);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditProvider>(newProvider);
        }

        public async Task<AddEditProvider> UpdateProvider(int id, AddEditProvider provider)
        {
            var oldCustomer = await _dbContext.Providers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el proveedor a editar.");

            var newProvider = _mapper.Map<Provider>(provider);

            _dbContext.Providers.Update(newProvider);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditProvider>(newProvider);
        }

        public async Task<bool> RemoveProvider(int id)
        {
            var existInventory = await _dbContext.InventoryEntries.AnyAsync(p => p.ProviderId == id);
            if (existInventory)
                throw new Exception("El proveedor no puede ser eliminado porque existen registros asociados a el.");

            var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el proveedor a eliminar.");

            _dbContext.Providers.Remove(provider);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
