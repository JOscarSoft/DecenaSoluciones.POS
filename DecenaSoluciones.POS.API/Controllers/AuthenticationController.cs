using DecenaSoluciones.POS.API.Helper;
using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DecenaSoluciones.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICompanyService _companyService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(
            IAuthService authService, 
            ICompanyService companyService, 
            IOptions<AppSettings> options)
        {
            _authService = authService;
            _companyService = companyService;
            _appSettings = options.Value;
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

        [HttpGet]
        [Route("users/company/{companyid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetCompanyUsersList(int companyid)
        {
            var apiResponse = new ApiResponse<List<RegistrationViewModel>>();
            try
            {
                var result = await _authService.GetCompanyUsersList(companyid);

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

        [HttpPost]
        [Route("anonymousregister")]
        public async Task<IActionResult> Register(AnonymousRegistrationViewModel model)
        {
            var apiResponse = new ApiResponse<AnonymousRegistrationResults>() { Result = new() };
            int companyId = model.CompanyId ?? 0;

            try
            {

                if(model.CompanyId == null || 
                    model.CompanyId <= 0)
                {
                    var companyCreationResult = await _companyService.AddNewCompany(new AddEditCompany
                    {
                        Name = model.CompanyName,
                        ContactEmail = model.ContactEmail,
                        ContactName = $"{model.FirstName} {model.LastName}",
                        ContactPhone = model.ContactPhone,
                        SubscriptionExpiration = DateTime.Now.AddMonths(_appSettings.TestingPeriod)
                    });

                    companyId = companyCreationResult.Id;
                }

                apiResponse.Result.CompanyId = companyId;

                await _authService.Registration(new RegistrationViewModel
                {
                    CompanyId = companyId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    Username = model.Username,
                    Role = UserRoles.Admin
                });

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
