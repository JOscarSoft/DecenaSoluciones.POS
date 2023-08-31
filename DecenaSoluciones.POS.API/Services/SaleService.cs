using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public SaleService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AddEditSale>> GetSalesList()
        {
            var sales = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)
                .ThenInclude(p => p.Product)
                .ToListAsync();
            return _mapper.Map<List<AddEditSale>>(sales);
        }

        public async Task<AddEditSale> GetSaleById(int id)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditSale>(sale);
        }

        public async Task<AddEditSale> GetSaleByCode(string code)
        {
            var sale = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Code == code);
            return _mapper.Map<AddEditSale>(sale);
        }
        
        public async Task<AddEditSale> AddNewSale(Sale newSale)
        {
            try
            {
                foreach (var product in newSale.SaleProducts)
                    product.Product = null;

                newSale.Customer = null;
                newSale.Code = await GetSaleCode();
                _dbContext.Sale.Add(newSale);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AddEditSale>(newSale);

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<AddEditSale> UpdateSale(int id, Sale sale)
        {
            var oldSale = await _dbContext.Sale
                .Include(p => p.SaleProducts)
                .Include(p => p.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el cliente a editar.");

            foreach(var product in sale.SaleProducts)
                product.Product = null;

            sale.Customer = null;

            _dbContext.Sale.Update(sale);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();

            var Ids = oldSale.SaleProducts!
                .Where(p => !sale.SaleProducts.Any(n => n.Id == p.Id))
                .Select(p => p.Id);

            foreach(var saleProdId in Ids)
            {
                _dbContext.SaleProducts.Remove(oldSale.SaleProducts.FirstOrDefault(p=>p.Id == saleProdId));
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
                newSequence = lastSequence.Sequence++;


            _dbContext.SaleSequence.Add(new SaleSequence
            {
                Code = DateTime.Now.ToString("yyyyMM"),
                Sequence = newSequence
            });

            return $"{DateTime.Now:yyyyMM}{newSequence.ToString().PadLeft(6, '0')}";
        }
    }
}
