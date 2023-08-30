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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiResponse = new ApiResponse<List<CustomerViewModel>>();
            try
            {
                apiResponse.Result = await _customerService.GetCustomerList();
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
            var apiResponse = new ApiResponse<AddEditCustomer>();
            try
            {
                var product = await _customerService.GetCustomerById(id);

                if (product == null)
                    throw new Exception("Cliente no encontrado");

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
        public async Task<IActionResult> CreateNewCustomer(AddEditCustomer customer)
        {
            var apiResponse = new ApiResponse<AddEditCustomer>();
            try
            {
                apiResponse.Result = await _customerService.AddNewCustomer(customer);
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
        public async Task<IActionResult> UpdateCustomer(int id, AddEditCustomer customer)
        {
            var apiResponse = new ApiResponse<AddEditCustomer>();
            try
            {
                apiResponse.Result = await _customerService.UpdateCustomer(id, customer);
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
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _customerService.RemoveCustomer(id);
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
