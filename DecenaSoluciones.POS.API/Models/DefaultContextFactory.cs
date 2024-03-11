using DecenaSoluciones.POS.API.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DecenaSoluciones.POS.API.Models
{
    public class DefaultContextFactory : IDefaultContextFactory, IDesignTimeDbContextFactory<DecenaSolucionesDBContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DefaultContextFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public DecenaSolucionesDBContext CreateContext()
        {
            string company = string.Empty;
            
            if(_httpContextAccessor.HttpContext?.User != null)
                company = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "Company")?.Value;

            var options = new DbContextOptionsBuilder<DecenaSolucionesDBContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;

            return new DecenaSolucionesDBContext(options, string.IsNullOrWhiteSpace(company) ? null : int.Parse(company));
        }

        // add this code for the IDesignTimeDbContextFactory implementation
        public DecenaSolucionesDBContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<DecenaSolucionesDBContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;

            return new DecenaSolucionesDBContext(options);
        }
    }
}
