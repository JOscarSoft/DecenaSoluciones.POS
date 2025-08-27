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
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiResponse = new ApiResponse<List<AddEditProvider>>();
            try
            {
                apiResponse.Result = await _providerService.GetProvidersList();
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
            var apiResponse = new ApiResponse<AddEditProvider>();
            try
            {
                var provider = await _providerService.GetProviderById(id);

                if (provider == null)
                    throw new Exception("Proveedor no encontrado");

                apiResponse.Result = provider;
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
        public async Task<IActionResult> CreateNewProvider(AddEditProvider provider)
        {
            var apiResponse = new ApiResponse<AddEditProvider>();
            try
            {
                apiResponse.Result = await _providerService.AddNewProvider(provider);
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
        public async Task<IActionResult> UpdateProvider(int id, AddEditProvider provider)
        {
            var apiResponse = new ApiResponse<AddEditProvider>();
            try
            {
                apiResponse.Result = await _providerService.UpdateProvider(id, provider);
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
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _providerService.RemoveProvider(id);
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
