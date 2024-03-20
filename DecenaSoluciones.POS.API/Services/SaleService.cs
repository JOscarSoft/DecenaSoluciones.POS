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
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .ToListAsync();
            return _mapper.Map<List<SalesViewModel>>(sales);
        }

        public async Task<AddEditSale> GetSaleById(int id)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditSale>(sale);
        }

        public async Task<AddEditSale> GetSaleByCode(string code)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Code == code);
            return _mapper.Map<AddEditSale>(sale);
        }
        
        public async Task<AddEditSale> AddNewSale(Sale newSale)
        {
            foreach (var product in newSale.SaleProducts!)
                product.Product = null;

            foreach (var item in newSale.SaleProducts!)
            {
                await _productService.UpdateProductStock(item.ProductId, item.Quantity * -1);

                if(newSale.CustomerId.HasValue)
                    await _customerService.AddProductToCustomer(item.ProductId, newSale.CustomerId.Value, item.Quantity);
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

            foreach (var item in newProducts)
            {
                await _productService.UpdateProductStock(item.ProductId, item.Quantity * -1);

                if (sale.CustomerId.HasValue)
                    await _customerService.AddProductToCustomer(item.ProductId, sale.CustomerId.Value, item.Quantity);
            }

            foreach (var item in removedProducts)
                await _productService.UpdateProductStock(item.ProductId, item.Quantity);

            foreach (var item in existingProducts)
            {
                var newQuantity = item.Quantity - oldSale.SaleProducts!.FirstOrDefault(p => p.ProductId == item.ProductId)!.Quantity;
                await _productService.UpdateProductStock(item.ProductId, newQuantity * -1);
            }

            _dbContext.Sale.Update(sale);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();

            var Ids = oldSale.SaleProducts!
                .Where(p => !sale.SaleProducts.Any(n => n.Id == p.Id))
                .Select(p => p.Id);

            foreach(var saleProdId in Ids)
            {
                _dbContext.SaleProducts.Remove(oldSale.SaleProducts!.FirstOrDefault(p=>p.Id == saleProdId)!);
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditSale>(sale);
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
