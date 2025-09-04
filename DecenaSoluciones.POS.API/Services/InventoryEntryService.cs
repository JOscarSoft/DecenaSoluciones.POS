using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class InventoryEntryService : IInventoryEntryService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public InventoryEntryService(DecenaSolucionesDBContext dbContext, IProductService productService, ICustomerService customerService, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
        }

        public async Task<List<InventoryEntryViewModel>> GetInventoryEntryList()
        {
            var inventoryEntries = await _dbContext.InventoryEntries
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();
            return _mapper.Map<List<InventoryEntryViewModel>>(inventoryEntries);
        }

        public async Task<InventoryEntryViewModel> GetInventoryEntryById(int id)
        {
            var inventoryEntry = await _dbContext.InventoryEntries
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }
        
        public async Task<InventoryEntryViewModel> AddNewInventoryEntry(InventoryEntry inventoryEntry)
        {
            foreach (var product in inventoryEntry.InventoryEntryDetails!)
                product.Product = null;


            var updateInventoryDto = _mapper.Map<List<UpdateInventory>>(inventoryEntry.InventoryEntryDetails);
            if(inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)
            {
                updateInventoryDto.ForEach(p =>
                {
                    p.Quantity *= -1;
                });
            }
            await _productService.UpdateInventary(updateInventoryDto);

            _dbContext.ChangeTracker.Clear();

            inventoryEntry.Provider = null;
            _dbContext.InventoryEntries.Add(inventoryEntry);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }

        public async Task<InventoryEntryViewModel> UpdateInventoryEntry(int id, InventoryEntry inventoryEntry)
        {
            _dbContext.ChangeTracker.Clear();
            var oldEntry = await _dbContext.InventoryEntries
                .Include(p => p.InventoryEntryDetails)
                .Include(p => p.Provider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la entrada de inventario a editar.");

            foreach (var product in inventoryEntry.InventoryEntryDetails!)
                product.Product = null;

            //First we rollback the previous in or out, increasing or decreasind the existing products
            var existingProducts = oldEntry.InventoryEntryDetails!.Where(p => oldEntry.InventoryEntryDetails!.Select(p => p.ProductId).Contains(p.ProductId));
            var updateInventoryDto = _mapper.Map<List<UpdateInventory>>(existingProducts);
            updateInventoryDto.ForEach(p =>
            {
                p.Quantity *= (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out ? 1 : -1);
            });

            await _productService.UpdateInventary(updateInventoryDto);
            _dbContext.ChangeTracker.Clear();

            //Now lets do the new in or out of products
            updateInventoryDto = _mapper.Map<List<UpdateInventory>>(inventoryEntry.InventoryEntryDetails);
            if (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)
            {
                updateInventoryDto.ForEach(p =>
                {
                    p.Quantity *= -1;
                });
            }
            await _productService.UpdateInventary(updateInventoryDto);
            _dbContext.ChangeTracker.Clear();


            inventoryEntry.Provider = null;
            _dbContext.InventoryEntries.Update(inventoryEntry);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }

        public async Task<bool> RemoveInventoryEntry(int id)
        {
            var inventoryEntry = await _dbContext.InventoryEntries
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la entrada de inventario a eliminar.");

            inventoryEntry.Provider = null;
            foreach (var product in inventoryEntry.InventoryEntryDetails!)
            {
                product.Product = null;
                _dbContext.InventoryEntryDetails.Remove(product);
            }

            _dbContext.InventoryEntries.Remove(inventoryEntry);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
