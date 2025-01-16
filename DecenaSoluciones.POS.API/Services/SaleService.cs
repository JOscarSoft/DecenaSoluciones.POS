using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public SaleService(DecenaSolucionesDBContext dbContext, IProductService productService, ICustomerService customerService, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<List<SalesViewModel>> GetSalesList()
        {
            var sales = await _dbContext.Sale
                .Include(p => p.DismissedSale)
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();
            return _mapper.Map<List<SalesViewModel>>(sales);
        }

        public async Task<AddEditSale> GetSaleById(int id)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.DismissedSale)
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            var result = _mapper.Map<AddEditSale>(sale);
            if (result != null)
            {
                result.originalSaleCode = (await _dbContext.Sale.FirstOrDefaultAsync(p => p.DismissedBySaleId == sale.Id))?.Code;
            }

            return result;
        }

        public async Task<AddEditSale> GetSaleByCode(string code)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.DismissedSale)
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Code == code);

            var result = _mapper.Map<AddEditSale>(sale);
            if (result != null)
            {
                result.originalSaleCode = (await _dbContext.Sale.FirstOrDefaultAsync(p => p.DismissedBySaleId == sale.Id))?.Code;
            }

            return result;
        }
        
        public async Task<AddEditSale> AddNewSale(Sale newSale)
        {
            foreach (var product in newSale.SaleProducts!)
                product.Product = null;

            var groupedProducts = newSale.SaleProducts!
                .GroupBy(p => p.ProductId)
                .Select(p => 
                new { 
                    productId = p.Key, 
                    quantity = p.Sum(a => a.Quantity)
                });

            foreach (var item in groupedProducts)
            {
                await _productService.UpdateProductStock(item.productId, item.quantity * -1);

                if(newSale.CustomerId.HasValue)
                    await _customerService.AddProductToCustomer(item.productId, newSale.CustomerId.Value, item.quantity);
            }

            _dbContext.ChangeTracker.Clear();

            newSale.Customer = null;
            newSale.Code = await GetSaleCode();
            _dbContext.Sale.Add(newSale);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditSale>(newSale);
        }

        public async Task<AddEditSale> UpdateSale(int id, Sale sale)
        {
            _dbContext.ChangeTracker.Clear();
            var oldSale = await _dbContext.Sale
                .Include(p => p.SaleProducts)
                .Include(p => p.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la venta a editar.");

            foreach(var product in sale.SaleProducts!)
                product.Product = null;

            sale.Customer = null;

            var newProducts = sale.SaleProducts!.Where(p => !oldSale.SaleProducts!.Select(p => p.ProductId).Contains(p.ProductId));
            var existingProducts = sale.SaleProducts!.Where(p => oldSale.SaleProducts!.Select(p => p.ProductId).Contains(p.ProductId));
            var removedProducts = oldSale.SaleProducts!.Where(p => !sale.SaleProducts!.Select(p => p.ProductId).Contains(p.ProductId));

            var groupedProducts = newProducts!
                .GroupBy(p => p.ProductId)
                .Select(p =>
                new {
                    productId = p.Key,
                    quantity = p.Sum(a => a.Quantity)
                });

            foreach (var item in groupedProducts)
            {
                await _productService.UpdateProductStock(item.productId, item.quantity * -1);

                if (sale.CustomerId.HasValue)
                    await _customerService.AddProductToCustomer(item.productId, sale.CustomerId.Value, item.quantity);
            }

            groupedProducts = removedProducts!
                .GroupBy(p => p.ProductId)
                .Select(p =>
                new {
                    productId = p.Key,
                    quantity = p.Sum(a => a.Quantity)
                });

            foreach (var item in groupedProducts)
                await _productService.UpdateProductStock(item.productId, item.quantity);


            groupedProducts = existingProducts!
                .GroupBy(p => p.ProductId)
                .Select(p =>
                new {
                    productId = p.Key,
                    quantity = p.Sum(a => a.Quantity)
                });

            foreach (var item in groupedProducts)
            {
                var newQuantity = item.quantity - oldSale.SaleProducts!.Where(p => p.ProductId == item.productId)!.Sum(j => j.Quantity);
                await _productService.UpdateProductStock(item.productId, newQuantity * -1);
            }

            sale.Id = 0;
            sale.Code = await GetSaleCode();
            sale.Dismissed = false;
            sale.CreationDate = DateTime.Now;
            foreach (var item in sale.SaleProducts)
                item.Id = 0;

            await _dbContext.Sale.AddAsync(sale);
            await _dbContext.SaveChangesAsync();

            _dbContext.ChangeTracker.Clear();

            oldSale.Dismissed = true;
            oldSale.DismissedBySaleId = sale.Id;
            _dbContext.Sale.Update(oldSale);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<AddEditSale>(sale);
            result.originalSaleCode = oldSale.Code;
            return result;
        }

        public async Task<AddEditSale> DismissSale(int id)
        {
            _dbContext.ChangeTracker.Clear();
            var oldSale = await _dbContext.Sale
                .Include(p => p.SaleProducts)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la venta a desestimar.");

            var groupedProducts = oldSale.SaleProducts!
                .GroupBy(p => p.ProductId)
                .Select(p =>
                new {
                    productId = p.Key,
                    quantity = p.Sum(a => a.Quantity)
                });

            foreach (var item in groupedProducts)
                await _productService.UpdateProductStock(item.productId, item.quantity);

            oldSale.Dismissed = true;
            _dbContext.Sale.Update(oldSale);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditSale>(oldSale);;
        }

        private async Task<string> GetSaleCode()
        {
            int newSequence = 1;
            string sequenceCode = DateTime.Now.ToString("yyyyMM");
            var lastSequence = await _dbContext.SaleSequence.FirstOrDefaultAsync(p => p.Code == sequenceCode);

            if (lastSequence != null)
            {
                newSequence = lastSequence.Sequence + 1;
                lastSequence.Sequence = newSequence;
                _dbContext.SaleSequence.Update(lastSequence);
            }
            else
            {
                _dbContext.SaleSequence.Add(new SaleSequence
                {
                    Code = sequenceCode,
                    Sequence = newSequence
                });
            }

            await _dbContext.SaveChangesAsync();

            return $"{DateTime.Now:yyyyMM}{newSequence.ToString().PadLeft(6, '0')}";
        }
    }
}
