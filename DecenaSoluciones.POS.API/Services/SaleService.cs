using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DecenaSoluciones.POS.API.Services
{
    public class SaleService (
        DecenaSolucionesDBContext dbContext, 
        IProductService productService,
        ICustomerService customerService,
        IMapper mapper) : ISaleService
    {
        private readonly DecenaSolucionesDBContext _dbContext = dbContext;
        private readonly IProductService _productService = productService;
        private readonly ICustomerService _customerService = customerService;
        private readonly IMapper _mapper = mapper;

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

        public async Task<GridResponse<SalesViewModel>> GetFilteredSalesList(GridRequest request)
        {
            var query = _dbContext.Sale
                .Include(p => p.DismissedSale)
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .AsQueryable();

            foreach (var filter in request.Filters)
            {
                var field = filter.Field;
                var value = filter.Value;

                if (filter.Operator == FilterOperator.None || filter.Operator == FilterOperator.Clear || field == "Total")
                    continue;


                if (field == "CustomerName")
                {
                    field = "(Customer.Name + Customer.LastName)";
                }
                else if(field == "CreationDate")
                {
                    value = !string.IsNullOrWhiteSpace(value) && value.Length > 10 ? value[..10] : value;
                    field = "CreationDate.Date";
                }

                switch (filter.Operator)
                {
                    case FilterOperator.Equals:
                        query = query.Where($"{field} == @0", value);
                        break;
                    case FilterOperator.NotEquals:
                        query = query.Where($"{field} != @0", value);
                        break;
                    case FilterOperator.LessThan:
                        query = query.Where($"{field} < @0", value);
                        break;
                    case FilterOperator.LessThanOrEquals:
                        query = query.Where($"{field} <= @0", value);
                        break;
                    case FilterOperator.GreaterThan:
                        query = query.Where($"{field} > @0", value);
                        break;
                    case FilterOperator.GreaterThanOrEquals:
                        query = query.Where($"{field} >= @0", value);
                        break;
                    case FilterOperator.Contains:
                        query = query.Where($"{field}.Contains(@0)", value);
                        break;
                    case FilterOperator.StartsWith:
                        query = query.Where($"{field}.StartsWith(@0)", value);
                        break;
                    case FilterOperator.EndsWith:
                        query = query.Where($"{field}.EndsWith(@0)", value);
                        break;
                    case FilterOperator.DoesNotContain:
                        query = query.Where($"!{field}.Contains(@0)", value);
                        break;
                    case FilterOperator.IsNull:
                        query = query.Where($"{field} == null");
                        break;
                    case FilterOperator.IsNotNull:
                        query = query.Where($"{field} != null");
                        break;
                    case FilterOperator.IsEmpty:
                        query = query.Where($"{field} == \"\"");
                        break;
                    case FilterOperator.IsNotEmpty:
                        query = query.Where($"{field} != \"\"");
                        break;
                }
            }
            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                if (request.SortColumn == "CustomerName")
                {
                    request.SortColumn = "(Customer.Name + Customer.LastName)";
                }

                var sort = $"{request.SortColumn} {request.SortDirection ?? "asc"}";
                query = query.OrderBy(sort);
            }
            else
            {
                query = query.OrderByDescending(p => p.CreationDate);
            }

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new GridResponse<SalesViewModel>
            {
                Items = _mapper.Map<List<SalesViewModel>>(items),
                TotalCount = totalCount
            };
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
                result.originalSaleCode = (await _dbContext.Sale.FirstOrDefaultAsync(p => p.DismissedBySaleId == sale!.Id))?.Code;
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
                result.originalSaleCode = (await _dbContext.Sale.FirstOrDefaultAsync(p => p.DismissedBySaleId == sale!.Id))?.Code;
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
