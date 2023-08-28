using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name:"Customers")]
    public class Customer : BaseEntity
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public long? PhoneNumber { get; set; }

        public string? Direction { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<CustomerProduct>? CustomerProducts { get; set; }

        public virtual ICollection<Quotation>? CustomerQuotations { get; set; }
    }
}
