using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "CustomerProducts")]
    public class CustomerProduct : BaseEntity, ICompanyEntity
    {
        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public DateTime? LastMaintenance { get; set; }

        public DateTime? NextMaintenance { get; set; }

        public DateTime? WarrantyEndDate { get; set; }

        public bool SoldByUs { get; set; }

        public bool HasWarranty { get; set; }

        public bool NeedMaintenance { get; set; }

        [MaxLength(50)]
        public string? Serial { get; set; }

        public DateTime? SaleDate { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual required Customer Customer { get; set; }

        public virtual required Product Product { get; set; }
    }
}
