using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DecenaSoluciones.POS.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly DecenaSolucionesDBContext _dbContext;

        public AuthService(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            DecenaSolucionesDBContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<bool> Registration(RegistrationViewModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                throw new Exception("Nombre de usuario no disponible.");

            AppUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username.ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                CompanyId = model.CompanyId > 0 ? model.CompanyId : null
            };

            var createUserResult = await userManager.CreateAsync(user, model.Password!);
            if (!createUserResult.Succeeded)
                throw new Exception(createUserResult.Errors?.FirstOrDefault()?.Description ?? "Error al crear el usuario.");

            if (!await roleManager.RoleExistsAsync(model.Role!))
                await roleManager.CreateAsync(new IdentityRole(model.Role!));

            if (await roleManager.RoleExistsAsync(model.Role!))
                await userManager.AddToRoleAsync(user, model.Role!);

            return createUserResult.Succeeded;
        }

        public async Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            if (model.Username.ToUpper().Equals("SUPERADMIN"))
                throw new Exception("Por motivos de seguridad este usuario no puede ser modificado.");

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists == null)
                throw new Exception("No se encontró el usuario especificado.");

            await userManager.RemovePasswordAsync(userExists);

            var changePasswordResult = await userManager.AddPasswordAsync(userExists, model.Password);
            if (!changePasswordResult.Succeeded)
                throw new Exception(changePasswordResult.Errors?.FirstOrDefault()?.Description ?? "Error al actualizar el usuario.");

            return changePasswordResult.Succeeded;
        }

        public async Task<bool> RemoveUser(string userName)
        {
            if(userName.ToUpper().Equals("SUPERADMIN"))
                throw new Exception("Por motivos de seguridad este usuario no puede ser eliminado.");

            var userExists = await userManager.FindByNameAsync(userName);
            if (userExists == null)
                throw new Exception("No se encontró el usuario especificado.");

            if (userExists.CompanyId.HasValue)
            {
                var moreUsers = await _dbContext.Users.AnyAsync(p => p.UserName != userExists.UserName && p.CompanyId == userExists.CompanyId);

                if(!moreUsers)
                    throw new Exception("No se puede eliminar el unico usuario existente para esta compañía.");
            }

            var deleteUserResult = await userManager.DeleteAsync(userExists);
            if (!deleteUserResult.Succeeded)
                throw new Exception(deleteUserResult.Errors?.FirstOrDefault()?.Description ?? "Error al eliminar el usuario.");

            return deleteUserResult.Succeeded;
        }

        public async Task<string> Login(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username.ToUpper());
            if (user == null)
                throw new Exception("Usuario inválido");
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception("Contraseña inválida");
            if(user.CompanyId != null && user.Company!.SubscriptionExpiration < DateTime.Now)
                throw new Exception("Su suscripción se encuentra inactiva, favor contactenos a traves de nuestro E-mail ventas@decenasoluciones.com");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            string companyId = user.CompanyId?.ToString() ?? model.CompanyId ?? "";
            authClaims.Add(new Claim("Company", companyId));
            return GenerateToken(authClaims, string.IsNullOrEmpty(companyId) || int.Parse(companyId) == 1 || int.Parse(companyId) == 0);
        }


        private string GenerateToken(IEnumerable<Claim> claims, bool dontExpire)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"] ?? "ecawiasqrpqrgyhwnolrudpbsrwaynbqdayndnmcehjnwqyouikpodzaqxivwkconwqbhrmxfgccbxbyljguwlxhdlcvxlutbnwjlgpfhjgqbegtbxbvwnacyqnltrby"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                Expires = dontExpire ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(6),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<List<RegistrationViewModel>> GetUsersList()
        {
            var result = new List<RegistrationViewModel>();
            var users = await userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                result.Add(new RegistrationViewModel
                {
                    Username = user.UserName!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRoles.FirstOrDefault(),
                    CompanyId = user.Company?.Id ?? 0,
                    CompanyName = user.Company?.Name ?? string.Empty,
                });
            }
            return result;
        }

        public async Task<List<RegistrationViewModel>> GetCompanyUsersList(int CompanyId)
        {
            var result = new List<RegistrationViewModel>();
            var users = await userManager.Users.Where(p => p.CompanyId == CompanyId).ToListAsync();
            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                result.Add(new RegistrationViewModel
                {
                    Username = user.UserName!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRoles.FirstOrDefault(),
                    CompanyId = user.Company?.Id ?? 0,
                    CompanyName = user.Company?.Name ?? string.Empty,
                });
            }
            return result;
        }
    }
}
