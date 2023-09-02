using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecenaSoluciones.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("users")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUsersList()
        {
            var apiResponse = new ApiResponse<List<RegistrationViewModel>>();
            try
            {
                var result = await _authService.GetUsersList();

                if (result == null)
                    throw new Exception("Error obteniendo lista de usuarios.");

                apiResponse.Result = result;
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
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var login = await _authService.Login(model);

                if (login == null)
                    throw new Exception("Error autenticando al usuario.");

                apiResponse.Result = login;
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
        [Route("newuser")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _authService.Registration(model);
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
        [Route("setpassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _authService.ChangePassword(model);
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
        [Route("removeuser/{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveUser(string userName)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                apiResponse.Result = await _authService.RemoveUser(userName);
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
