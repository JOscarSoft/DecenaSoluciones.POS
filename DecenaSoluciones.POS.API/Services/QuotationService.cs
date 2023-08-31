using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<AddEditSale>> GetQuotationsList()
        {
            var Quotations = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)
                .ThenInclude(p => p.Product)
                .ToListAsync();
            return _mapper.Map<List<AddEditSale>>(Quotations);
        }

        public async Task<AddEditSale> GetQuotationById(int id)
        {
            var Quotation = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditSale>(Quotation);
        }

        public async Task<AddEditSale> GetQuotationByCode(string code)
        {
            var Quotation = await _dbContext.Quotation
                .Include(p => p.Customer)
                .Include(p => p.QuotationProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Code == code);
            return _mapper.Map<AddEditSale>(Quotation);
        }
        
        public async Task<AddEditSale> AddNewQuotation(Quotation newQuotation)
        {
            foreach (var product in newQuotation.QuotationProducts)
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
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el cliente a editar.");

            foreach(var product in Quotation.QuotationProducts)
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
                _dbContext.QuotationProducts.Remove(oldQuotation.QuotationProducts.FirstOrDefault(p=>p.Id == QuotationProdId));
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
                newSequence = lastSequence.Sequence++;


            _dbContext.QuotationSequence.Add(new QuotationSequence
            {
                Code = DateTime.Now.ToString("yyyyMM"),
                Sequence = newSequence
            });

            return $"{DateTime.Now:yyyyMM}{newSequence.ToString().PadLeft(6, '0')}";
        }
    }
}
