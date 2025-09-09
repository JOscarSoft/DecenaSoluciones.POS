using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DecenaSoluciones.POS.API.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public QuotationService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<SalesViewModel>> GetQuotationsList()
        {
            var Quotations = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)!
                .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();
            return _mapper.Map<List<SalesViewModel>>(Quotations);
        }

        public async Task<GridResponse<SalesViewModel>> GetFilteredQuotationsList(GridRequest request)
        {
            var query = _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)!
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
                else if (field == "CreationDate")
                {
                    value = !string.IsNullOrWhiteSpace(value) && value.Length > 10 ? value.Substring(0, 10) : value;
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

        public async Task<AddEditSale> GetQuotationById(int id)
        {
            var Quotation = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)!
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditSale>(Quotation);
        }

        public async Task<AddEditSale> GetQuotationByCode(string code)
        {
            var Quotation = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)!
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Code == code);
            return _mapper.Map<AddEditSale>(Quotation);
        }
        
        public async Task<AddEditSale> AddNewQuotation(Quotation newQuotation)
        {
            foreach (var product in newQuotation.QuotationProducts!)
                product.Product = null;

            newQuotation.Customer = null;
            newQuotation.Code = await GetQuotationCode();
            _dbContext.Quotation.Add(newQuotation);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditSale>(newQuotation);
        }

        public async Task<AddEditSale> UpdateQuotation(int id, Quotation Quotation)
        {
            var oldQuotation = await _dbContext.Quotation
                .Include(p => p.QuotationProducts)
                .Include(p => p.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la cotización a editar.");

            foreach(var product in Quotation.QuotationProducts!)
                product.Product = null;

            Quotation.Customer = null;

            _dbContext.Quotation.Update(Quotation);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();

            var Ids = oldQuotation.QuotationProducts!
                .Where(p => !Quotation.QuotationProducts.Any(n => n.Id == p.Id))
                .Select(p => p.Id);

            foreach(var QuotationProdId in Ids)
            {
                _dbContext.QuotationProducts.Remove(oldQuotation.QuotationProducts!.FirstOrDefault(p=>p.Id == QuotationProdId)!);
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditSale>(Quotation);
        }

        private async Task<string> GetQuotationCode()
        {
            int newSequence = 1;
            string sequenceCode = DateTime.Now.ToString("yyyyMM");
            var lastSequence = await _dbContext.QuotationSequence.FirstOrDefaultAsync(p => p.Code == sequenceCode);

            if (lastSequence != null)
            {
                newSequence = lastSequence.Sequence + 1;
                lastSequence.Sequence = newSequence;
                _dbContext.QuotationSequence.Update(lastSequence);
            }
            else
            {
                _dbContext.QuotationSequence.Add(new QuotationSequence
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
