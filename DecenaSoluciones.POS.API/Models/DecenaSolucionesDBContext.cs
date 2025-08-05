using DecenaSoluciones.POS.API.Models.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace DecenaSoluciones.POS.API.Models
{
    public class DecenaSolucionesDBContext : IdentityDbContext<AppUser>
    {
        private readonly int? _companyId;

        public DecenaSolucionesDBContext(DbContextOptions<DecenaSolucionesDBContext> options, int? companyId):base(options) 
        {
            _companyId = companyId;
        }

        public DecenaSolucionesDBContext(DbContextOptions<DecenaSolucionesDBContext> options) : base(options) { }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<ICompanyEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.CompanyId = _companyId ?? 1;
                        break;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().Navigation(e => e.Company).AutoInclude();

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            
            Expression<Func<ICompanyEntity, bool>> filterExpr = bm => bm.CompanyId == (_companyId ?? bm.CompanyId);

            foreach (var mutableEntityType in builder.Model.GetEntityTypes())
            {
                // check if current entity type is child of BaseModel  
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(ICompanyEntity)))
                {
                    // modify expression to handle correct child type  
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    // set filter  
                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }

            builder.Entity<Sale>().HasOne(p => p.DismissedSale)
                                  .WithMany()
                                  .HasForeignKey(b => b.DismissedBySaleId);

            base.OnModelCreating(builder);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<Quotation> Quotation { get; set; }
        public DbSet<QuotationProduct> QuotationProducts { get; set; }
        public DbSet<QuotationSequence> QuotationSequence { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<SaleSequence> SaleSequence { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<InventoryEntryType> InventoryEntryTypes { get; set; }
        public DbSet<InventoryEntry> InventoryEntries { get; set; }
        public DbSet<InventoryEntryDetail> InventoryEntryDetails { get; set; }

    }
}
