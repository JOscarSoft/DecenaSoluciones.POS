using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Quotation")]
    public class Quotation : BaseEntity
    {
        public required string Code { get; set; }
        public int? CustomerId { get; set; }
        public decimal? WorkForceValue { get; set; }
        public decimal? Discount { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<QuotationProduct>? QuotationProducts { get; set; }
    }
}
