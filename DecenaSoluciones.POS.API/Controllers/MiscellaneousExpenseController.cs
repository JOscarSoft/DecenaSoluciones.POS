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
    public class MiscellaneousExpenseController : ControllerBase
    {
        private readonly IMiscellaneousExpenseService _miscExpenseService;
        public MiscellaneousExpenseController(IMiscellaneousExpenseService miscExpenseService)
        {
            _miscExpenseService = miscExpenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiResponse = new ApiResponse<List<AddEditMiscellaneousExpense>>();
            try
            {
                apiResponse.Result = await _miscExpenseService.GetMiscellaneousExpensesList();
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
            var apiResponse = new ApiResponse<AddEditMiscellaneousExpense>();
            try
            {
                var expense = await _miscExpenseService.GetMiscellaneousExpenseById(id);

                if (expense == null)
                    throw new Exception("Gasto no encontrado");

                apiResponse.Result = expense;
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
        public async Task<IActionResult> CreateNewMiscellaneousExpense(AddEditMiscellaneousExpense expense)
        {
            var apiResponse = new ApiResponse<AddEditMiscellaneousExpense>();
            try
            {
                apiResponse.Result = await _miscExpenseService.AddNewMiscellaneousExpense(expense);
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
        public async Task<IActionResult> UpdateMiscellaneousExpense(int id, AddEditMiscellaneousExpense expense)
        {
            var apiResponse = new ApiResponse<AddEditMiscellaneousExpense>();
            try
            {
                apiResponse.Result = await _miscExpenseService.UpdateMiscellaneousExpense(id, expense);
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
        public async Task<IActionResult> DeleteMiscellaneousExpense(int id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _miscExpenseService.RemoveMiscellaneousExpense(id);
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
