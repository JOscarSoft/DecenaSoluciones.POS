using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DecenaSoluciones.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiResponse = new ApiResponse<List<ProductViewModel>>();
            try
            {
                apiResponse.Result = await _productService.GetProductList();
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apiResponse = new ApiResponse<ProductViewModel>();
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                    throw new Exception("Producto no encontrado");

                apiResponse.Result = product;
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var apiResponse = new ApiResponse<ProductViewModel>();
            try
            {
                var product = await _productService.GetProductByCode(code);

                if (product == null)
                    throw new Exception("Producto no encontrado");

                apiResponse.Result = product;
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(AddEditProduct product)
        {
            var apiResponse = new ApiResponse<int>();
            try
            {
                apiResponse.Result = await _productService.AddNewProduct(product);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, AddEditProduct product)
        {
            var apiResponse = new ApiResponse<ProductViewModel>();
            try
            {
                apiResponse.Result = await _productService.UpdateProduct(id, product);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var apiResponse = new ApiResponse<int>();
            try
            {
                apiResponse.Result = await _productService.RemoveProduct(id);
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }
    }
}
