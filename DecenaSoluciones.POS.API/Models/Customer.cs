using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name:"Customers")]
    public class Customer : BaseEntity
    {
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Direction { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<CustomerProduct>? CustomerProducts { get; set; }

        public virtual ICollection<Quotation>? CustomerQuotations { get; set; }

        public virtual ICollection<Sale>? CustomerSales { get; set; }
    }
}
