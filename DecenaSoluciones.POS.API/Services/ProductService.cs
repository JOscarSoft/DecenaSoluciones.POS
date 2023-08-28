using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class ProductService : IProductService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> AddNewProduct(AddEditProduct productDto)
        {
            var existCode = await _dbContext.Products.AnyAsync(p => p.Code == productDto.Code);
            if (existCode)
                throw new Exception("Ya existe un producto con el código ingresado");

            var product = _mapper.Map<Product>(productDto);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task<ProductViewModel> GetProductByCode(string code)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Code == code);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> GetProductById(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<List<ProductViewModel>> GetProductList()
        {
            var product = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductViewModel>>(product);
        }

        public async Task<int> RemoveProduct(int id)
        {
            var existCustomer = await _dbContext.CustomerProducts.AnyAsync(p => p.ProductId == id);
            var existQuotation = await _dbContext.QuotationProducts.AnyAsync(p => p.ProductId == id);
            var existSale = await _dbContext.SaleProducts.AnyAsync(p => p.ProductId == id);
            if (existCustomer || existQuotation || existSale)
                throw new Exception("El producto no puede ser eliminado porque existen registros asociados a el.");

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el producto a eliminar.");

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return 1;
        }

        public async Task<ProductViewModel> UpdateProduct(int id, AddEditProduct productDto)
        {
            var oldProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el producto a editar.");

            var newProduct = _mapper.Map<Product>(productDto);
            newProduct.Id = id;
            _dbContext.Products.Update(newProduct);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProductViewModel>(newProduct);
        }
    }
}
