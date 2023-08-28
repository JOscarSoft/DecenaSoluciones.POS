using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Products")]
    public class Product : BaseEntity
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public int? MaintenancePeriods { get; set; }
        public int? WarrantyTime { get; set; }
        public required int stock { get; set; }
        public required decimal Price { get; set; }
        public required decimal Cost { get; set; }
        public bool Assignable { get; set; }
        public decimal ITBIS { get; set; }

        public virtual ICollection<CustomerProduct>? CustomerProducts { get; set; }
    }
}
