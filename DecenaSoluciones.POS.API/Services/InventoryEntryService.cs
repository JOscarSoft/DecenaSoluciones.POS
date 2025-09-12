using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DecenaSoluciones.POS.API.Services
{
    public class InventoryEntryService(
        DecenaSolucionesDBContext dbContext, 
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor) : IInventoryEntryService
    {
        public async Task<List<InventoryEntryViewModel>> GetInventoryEntryList()
        {
            var inventoryEntries = await dbContext.InventoryEntries
                .Include(p => p.InventoryEntryType)
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();
            return mapper.Map<List<InventoryEntryViewModel>>(inventoryEntries);
        }

        public async Task<InventoryEntryViewModel> GetInventoryEntryById(int id)
        {
            var inventoryEntry = await dbContext.InventoryEntries
                .Include(p => p.InventoryEntryType)
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }

        public async Task<InventoryEntryViewModel> AddNewInventoryEntry(InventoryEntry inventoryEntry)
        {
            foreach (var product in inventoryEntry.InventoryEntryDetails!)
                product.Product = null;


            var updateInventoryDto = mapper.Map<List<UpdateInventory>>(inventoryEntry.InventoryEntryDetails);
            if (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)
            {
                updateInventoryDto.ForEach(p =>
                {
                    p.Quantity *= -1;
                });
            }
            await UpdateProductStock(updateInventoryDto);

            dbContext.ChangeTracker.Clear();

            inventoryEntry.Provider = null;
            inventoryEntry.InventoryEntryType = null;
            dbContext.InventoryEntries.Add(inventoryEntry);

            await dbContext.SaveChangesAsync();

            return mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }

        public async Task<InventoryEntryViewModel> UpdateInventoryEntry(int id, InventoryEntry inventoryEntry)
        {
            dbContext.ChangeTracker.Clear();
            var oldEntry = await dbContext.InventoryEntries
                .Include(p => p.InventoryEntryDetails)
                .Include(p => p.Provider)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la entrada de inventario a editar.");

            foreach (var product in inventoryEntry.InventoryEntryDetails!)
                product.Product = null;

            //First we rollback the previous in or out, increasing or decreasind the existing products
            var existingProducts = oldEntry.InventoryEntryDetails!.Where(p => oldEntry.InventoryEntryDetails!.Select(p => p.ProductId).Contains(p.ProductId));
            var updateInventoryDto = mapper.Map<List<UpdateInventory>>(existingProducts);
            updateInventoryDto.ForEach(p =>
            {
                p.Quantity *= (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out ? 1 : -1);
            });

            await UpdateProductStock(updateInventoryDto);
            dbContext.ChangeTracker.Clear();

            //Now lets do the new in or out of products
            updateInventoryDto = mapper.Map<List<UpdateInventory>>(inventoryEntry.InventoryEntryDetails);
            if (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)
            {
                updateInventoryDto.ForEach(p =>
                {
                    p.Quantity *= -1;
                });
            }
            await UpdateProductStock(updateInventoryDto);
            dbContext.ChangeTracker.Clear();


            inventoryEntry.Provider = null;
            inventoryEntry.InventoryEntryType = null;
            dbContext.InventoryEntries.Update(inventoryEntry);
            await dbContext.SaveChangesAsync();

            return mapper.Map<InventoryEntryViewModel>(inventoryEntry);
        }

        public async Task<bool> RemoveInventoryEntry(int id)
        {
            var inventoryEntry = await dbContext.InventoryEntries
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la entrada de inventario a eliminar.");

            inventoryEntry.Provider = null;
            foreach (var product in inventoryEntry.InventoryEntryDetails!)
            {
                product.Product = null;
                dbContext.InventoryEntryDetails.Remove(product);
            }

            dbContext.InventoryEntries.Remove(inventoryEntry);
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> UpdateProductStock(List<UpdateInventory> inventoryItems)
        {
            foreach (var inventoryItem in inventoryItems)
            {
                var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == inventoryItem.productId || p.Code == inventoryItem.ProductCode);

                if (product != null)
                {
                    product.Cost = inventoryItem.Cost;
                    product.Price = inventoryItem.Price;
                    product.stock += inventoryItem.Quantity;

                    dbContext.Products.Update(product);
                }
            }
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task AddNewInventoryEntryFromProductChange(Product productModification, decimal difference)
        {
            try
            {

                dbContext.ChangeTracker.Clear();
                string userName = string.Empty;

                if (httpContextAccessor.HttpContext?.User != null)
                    userName = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? string.Empty;

                var inventoryEntry = new InventoryEntry()
                {
                    Id = 0,
                    InventoryEntryTypeId = (int)(difference > 0 ? Shared.Enums.InventoryEntryType.In : Shared.Enums.InventoryEntryType.Out),
                    CreationDate = DateTime.Now,
                    UserName = userName,
                    InventoryEntryDetails = new List<InventoryEntryDetail>()
                };

                difference = difference > 0 ? difference : difference * -1;
                var inventoryEntryDetail = new InventoryEntryDetail()
                {
                    ProductId = productModification.Id,
                    Quantity = difference,
                    UnitCost = productModification.Cost,
                    UnitPrice = productModification.Price,
                    Product = productModification,
                    InventoryEntry = inventoryEntry,
                    InventoryEntryId = productModification.Id,
                    TotalCost = productModification.Cost * difference,
                    Comments = "Modificación directa de stock"
                };

                inventoryEntryDetail.Product = null;

                var lastModEntry = await dbContext.InventoryEntries
                    .Include(p => p.InventoryEntryDetails)
                    .Include(p => p.Provider)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.In && p.ProviderId == null && p.CreationDate > DateTime.Today);

                if (inventoryEntry.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.In && lastModEntry != null)
                {
                    inventoryEntry = lastModEntry;
                    inventoryEntryDetail.InventoryEntry = null;
                    inventoryEntryDetail.InventoryEntryId = inventoryEntry.Id;

                    dbContext.InventoryEntryDetails.Add(inventoryEntryDetail);
                }
                else
                {
                    inventoryEntry.InventoryEntryDetails.Add(inventoryEntryDetail);
                    dbContext.InventoryEntries.Add(inventoryEntry);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
