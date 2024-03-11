using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name:"Customers")]
    public class Customer : BaseEntity, ICompanyEntity
    {
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Direction { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }

        public virtual ICollection<CustomerProduct>? CustomerProducts { get; set; }

        public virtual ICollection<Quotation>? CustomerQuotations { get; set; }

        public virtual ICollection<Sale>? CustomerSales { get; set; }
        public virtual Company Company { get; set; }
    }
}
