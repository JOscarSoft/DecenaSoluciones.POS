using AutoMapper;
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
    public class SaleController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly ISaleService _saleService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        public SaleController(IQuotationService quotationService, ISaleService saleService, ICustomerService customerService, IMapper mapper)
        {
            _quotationService = quotationService;
            _saleService = saleService;
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var apiResponse = new ApiResponse<List<SalesViewModel>>();
            try
            {
                apiResponse.Result = await _saleService.GetSalesList();
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
        [Route("quotation")]
        public async Task<IActionResult> GetAllQuotations()
        {
            var apiResponse = new ApiResponse<List<SalesViewModel>>();
            try
            {
                apiResponse.Result = await _quotationService.GetQuotationsList();
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
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {
                var sale = await _saleService.GetSaleById(id);

                if (sale == null)
                    throw new Exception("Venta no encontrada");

                apiResponse.Result = sale;
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
        [Route("quotation/{id}")]
        public async Task<IActionResult> GetQuotationById(int id)
        {
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {
                var quotation = await _quotationService.GetQuotationById(id);

                if (quotation == null)
                    throw new Exception("Cotización no encontrada");

                apiResponse.Result = quotation;
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
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {
                var sale = await _saleService.GetSaleByCode(code);

                if (sale == null)
                    throw new Exception("Venta no encontrado");

                apiResponse.Result = sale;
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
        [Route("quotation/GetByCode/{code}")]
        public async Task<IActionResult> GetQuotationByCode(string code)
        {
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {
                var quotation = await _quotationService.GetQuotationByCode(code);

                if (quotation == null)
                    throw new Exception("Cotización no encontrado");

                apiResponse.Result = quotation;
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
        public async Task<IActionResult> CreateNewSale(AddEditSale sale)
        {
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {
                sale.CustomerId = sale.Customer!.Id;

                if (sale.Customer.Id == 0 && !string.IsNullOrEmpty(sale.Customer.Name))
                {
                    var newCustomer = await _customerService.AddNewCustomer(sale.Customer);
                    sale.Customer.Id = newCustomer.Id;
                    sale.CustomerId = newCustomer.Id;
                }
                
                if (sale.CustomerId == 0)
                    sale.CustomerId = null;

                if (sale.IsAQuotation)
                    apiResponse.Result = await _quotationService.AddNewQuotation(_mapper.Map<Quotation>(sale));
                else
                    apiResponse.Result = await _saleService.AddNewSale(_mapper.Map<Sale>(sale));

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
        public async Task<IActionResult> UpdateSale(int id, AddEditSale sale)
        {
            var apiResponse = new ApiResponse<AddEditSale>();
            try
            {

                sale.CustomerId = sale.Customer?.Id;

                if (sale.Customer != null && sale.Customer.Id == 0 && !string.IsNullOrEmpty(sale.Customer.Name))
                {
                    var newCustomer = await _customerService.AddNewCustomer(sale.Customer);
                    sale.Customer.Id = newCustomer.Id;
                    sale.CustomerId = newCustomer.Id;
                }

                if (sale.CustomerId == 0)
                    sale.CustomerId = null;

                if (sale.IsAQuotation)
                    apiResponse.Result = await _quotationService.UpdateQuotation(id, _mapper.Map<Quotation>(sale));
                else
                    apiResponse.Result = await _saleService.UpdateSale(id, _mapper.Map<Sale>(sale));

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
