using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DecenaSoluciones.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryEntryController : ControllerBase
    {
        private readonly IInventoryEntryService _InventoryEntryService;
        private readonly IMapper _mapper;

        public InventoryEntryController(IInventoryEntryService inventoryEntryService, IMapper mapper)
        {
            _InventoryEntryService = inventoryEntryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiResponse = new ApiResponse<List<InventoryEntryViewModel>>();
            try
            {
                apiResponse.Result = await _InventoryEntryService.GetInventoryEntryList();
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
            var apiResponse = new ApiResponse<InventoryEntryViewModel>();
            try
            {
                var InventoryEntry = await _InventoryEntryService.GetInventoryEntryById(id);

                if (InventoryEntry == null)
                    throw new Exception("Entrada de inventario no encontrada");

                apiResponse.Result = InventoryEntry;
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
        [Authorize(Roles = $"{UserRoles.Manager},{UserRoles.Admin}")]
        public async Task<IActionResult> CreateNewInventoryEntry(InventoryEntryViewModel InventoryEntry)
        {
            var apiResponse = new ApiResponse<InventoryEntryViewModel>();
            try
            {
                apiResponse.Result = await _InventoryEntryService.AddNewInventoryEntry(_mapper.Map<InventoryEntry>(InventoryEntry));
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
        [Authorize(Roles = $"{UserRoles.Manager},{UserRoles.Admin}")]
        public async Task<IActionResult> UpdateInventoryEntry(int id, InventoryEntryViewModel InventoryEntry)
        {
            var apiResponse = new ApiResponse<InventoryEntryViewModel>();
            try
            {
                apiResponse.Result = await _InventoryEntryService.UpdateInventoryEntry(id, _mapper.Map<InventoryEntry>(InventoryEntry));
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
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteInventoryEntry(int id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _InventoryEntryService.RemoveInventoryEntry(id);
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
