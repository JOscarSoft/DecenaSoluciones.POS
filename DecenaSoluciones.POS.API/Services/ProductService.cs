using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DecenaSoluciones.POS.API.Services
{
    public class ProductService(DecenaSolucionesDBContext dbContext, IMapper mapper, IInventoryEntryService inventoryEntryService) : IProductService
    {
        public async Task<ProductViewModel> AddNewProduct(AddEditProduct productDto)
        {
            var existCode = await dbContext.Products.AnyAsync(p => p.Code == productDto.Code);
            if (existCode)
                throw new Exception("Ya existe un producto con el código ingresado");

            if(string.IsNullOrEmpty(productDto.Code))
                productDto.Code = await GenerateCodeFromDescription(productDto.Description);

            var product = mapper.Map<Product>(productDto);
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            
            await inventoryEntryService.AddNewInventoryEntryFromProductChange(product, product.stock);

            return mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> GetProductByCode(string code)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Code == code);
            return mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> GetProductById(int id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            return mapper.Map<ProductViewModel>(product);
        }

        public async Task<List<ProductViewModel>> GetProductList()
        {
            var product = await dbContext.Products.ToListAsync();
            return mapper.Map<List<ProductViewModel>>(product);
        }

        public async Task<LastSaleXProductViewModel> GetLastSaleXProduct(int id)
        {
            var saleProduct = await dbContext
                .SaleProducts
                .Include(p => p.Sale)
                .OrderByDescending(p => p.Sale.CreationDate)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return mapper.Map<LastSaleXProductViewModel>(saleProduct);
        }

        public async Task<List<ProductViewModel>> GetAssignables()
        {
            var product = await dbContext.Products.Where(p => p.Assignable).ToListAsync();
            return mapper.Map<List<ProductViewModel>>(product);
        }

        public async Task<int> RemoveProduct(int id)
        {
            var existCustomer = await dbContext.CustomerProducts.AnyAsync(p => p.ProductId == id);
            var existQuotation = await dbContext.QuotationProducts.AnyAsync(p => p.ProductId == id);
            var existSale = await dbContext.SaleProducts.AnyAsync(p => p.ProductId == id);
            var existInventory = await dbContext.InventoryEntryDetails.AnyAsync(p => p.ProductId == id);
            if (existCustomer || existQuotation || existSale || existInventory)
                throw new Exception("El producto no puede ser eliminado porque existen registros asociados a el.");

            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el producto a eliminar.");

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();

            return 1;
        }

        public async Task<ProductViewModel> UpdateProduct(int id, AddEditProduct productDto)
        {
            var existCode = await dbContext.Products.AnyAsync(p => p.Code == productDto.Code && p.Id != id);
            if (existCode)
                throw new Exception("Ya existe un producto con el código ingresado");

            var oldProduct = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el producto a editar.");

            if (string.IsNullOrEmpty(productDto.Code))
                productDto.Code = await GenerateCodeFromDescription(productDto.Description);

            var newProduct = mapper.Map<Product>(productDto);
            newProduct.Id = id;
            dbContext.Products.Update(newProduct);
            await dbContext.SaveChangesAsync();

            decimal stockDiff = productDto.stock - oldProduct.stock;
            if(stockDiff != 0)
            {
                await inventoryEntryService.AddNewInventoryEntryFromProductChange(newProduct, stockDiff);
            }

            return mapper.Map<ProductViewModel>(newProduct);
        }

        public async Task<ProductViewModel> UpdateProductStock(int id, decimal quantity)
        {
            var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el producto a editar.");

            product.stock += quantity;
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();

            return mapper.Map<ProductViewModel>(product);
        }

        private async Task<string> GenerateCodeFromDescription(string description)
        {
            string plainText = Regex.Replace(description, "[^a-zA-Z]+", "", RegexOptions.Compiled);
            string init = plainText.Length > 3 ? plainText.Substring(0, 3) : plainText;
            var existingCodes = await dbContext.Products.Where(p => p.Code.Contains(init)).ToListAsync();
            var codeDigits = existingCodes.Select(p => new string(p.Code.Where(Char.IsDigit).ToArray()))
                                          .Where(p => !string.IsNullOrEmpty(p))
                                          .Select(p => int.Parse(p)).ToList();
            if (codeDigits.Any())
                return $"{init}{(codeDigits.Max() + 1).ToString().PadLeft(4, '0')}".ToUpper();

            return $"{init}0001".ToUpper();
        }
    }
}
