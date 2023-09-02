using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Models
{
    public class DecenaSolucionesDBContext : IdentityDbContext<AppUser>
    {
        public DecenaSolucionesDBContext(DbContextOptions<DecenaSolucionesDBContext> options):base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<Quotation> Quotation { get; set; }
        public DbSet<QuotationProduct> QuotationProducts { get; set; }
        public DbSet<QuotationSequence> QuotationSequence { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<SaleSequence> SaleSequence { get; set; }
    }
}
