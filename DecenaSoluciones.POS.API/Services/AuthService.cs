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
        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

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
                EmailConfirmed = true
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                throw new Exception(createUserResult.Errors?.FirstOrDefault()?.Description ?? "Error al crear el usuario.");

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
                await userManager.AddToRoleAsync(user, model.Role);

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

           

            var changePasswordResult = await userManager.DeleteAsync(userExists);
            if (!changePasswordResult.Succeeded)
                throw new Exception(changePasswordResult.Errors?.FirstOrDefault()?.Description ?? "Error al eliminar el usuario.");

            return changePasswordResult.Succeeded;
        }

        public async Task<string> Login(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username.ToUpper());
            if (user == null)
                throw new Exception("Usuario inválido");
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception("Contraseña inválida");
            if(user.CompanyId != null && !user.Company!.Active)
                throw new Exception("Su suscripción se encuentra inactiva, favor contactenos!");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            authClaims.Add(new Claim("Company", user.CompanyId?.ToString() ?? model.CompanyId?.ToString() ?? ""));
            return GenerateToken(authClaims);
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"] ?? "ecawiasqrpqrgyhwnolrudpbsrwaynbqdayndnmcehjnwqyouikpodzaqxivwkconwqbhrmxfgccbxbyljguwlxhdlcvxlutbnwjlgpfhjgqbegtbxbvwnacyqnltrby"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                Expires = DateTime.Now.AddYears(1),
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
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRoles.FirstOrDefault()
                });
            }
            return result;
        }
    }
}
