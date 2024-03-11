using DecenaSoluciones.POS.API.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Products")]
    public class Product : BaseEntity, ICompanyEntity
    {
        [MaxLength(15)]
        public required string Code { get; set; }
        [MaxLength(250)]
        public required string Description { get; set; }
        public int? MaintenancePeriods { get; set; }
        public int? WarrantyTime { get; set; }
        public required int stock { get; set; }
        public required decimal Price { get; set; }
        public required decimal Cost { get; set; }
        public bool Assignable { get; set; }
        public decimal ITBIS { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<CustomerProduct>? CustomerProducts { get; set; }
    }
}
