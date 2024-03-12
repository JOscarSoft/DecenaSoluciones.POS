using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DecenaSoluciones.POS.API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public CompanyService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CompanyViewModel>> GetCompanyList()
        {
            var companies = await _dbContext.Companies.ToListAsync();
            return _mapper.Map<List<CompanyViewModel>>(companies);
        }

        public async Task<CompanyViewModel> AddNewCompany(AddEditCompany companyDto)
        {
            var existCompany = await _dbContext.Companies
                .AnyAsync(p => p.Name.ToLower().Trim() == companyDto.Name.ToLower().Trim());

            if (existCompany)
                throw new Exception("Ya existe una compañía con el nombre ingresado");

            var company = _mapper.Map<Company>(companyDto);
            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CompanyViewModel>(company);
        }

        public async Task<CompanyViewModel> UpdateCompany(int id, AddEditCompany companyDto)
        {
            var existCompany = await _dbContext.Companies
                .AnyAsync(p => p.Name.ToLower().Trim() == companyDto.Name.ToLower().Trim() && p.Id != id);

            if (existCompany)
                throw new Exception("Ya existe una compañía con el nombre ingresado");

            var oldProduct = await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la compañía a editar.");

            var newCompany = _mapper.Map<Company>(companyDto);
            newCompany.Id = id;
            _dbContext.Companies.Update(newCompany);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CompanyViewModel>(newCompany);
        }

        public async Task<int> RemoveCompany(int id)
        {
            var existCustomerProduct = await _dbContext.CustomerProducts.AnyAsync(p => p.CompanyId == id);
            var existQuotationProduct = await _dbContext.QuotationProducts.AnyAsync(p => p.CompanyId == id);
            var existSaleProduct = await _dbContext.SaleProducts.AnyAsync(p => p.CompanyId == id);
            var existUser = await _dbContext.Users.AnyAsync(p => p.CompanyId == id);
            var existCustomer = await _dbContext.Customers.AnyAsync(p => p.CompanyId == id);
            var existProduct = await _dbContext.Products.AnyAsync(p => p.CompanyId == id);
            var existQuotation = await _dbContext.Quotation.AnyAsync(p => p.CompanyId == id);
            var existSale = await _dbContext.Sale.AnyAsync(p => p.CompanyId == id);

            if (existCustomerProduct ||
                existQuotationProduct ||
                existSaleProduct ||
                existUser ||
                existCustomer ||
                existProduct ||
                existQuotation ||
                existSale)
                throw new Exception("La compañía no puede ser eliminada porque existen registros asociados a ella.");

            var company = await _dbContext.Companies.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró la compañía a eliminar.");

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();

            return 1;
        }
    }
}
