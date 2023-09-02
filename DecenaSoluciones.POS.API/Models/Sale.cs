using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Sales")]
    public class Sale : BaseEntity
    {
        [MaxLength(25)]
        public required string Code { get; set; }
        public int? CustomerId { get; set; }
        public decimal? WorkForceValue { get; set; }
        public decimal? Discount { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? DepositsAmount { get; set; }
        public decimal? TCAmount { get; set; }
        [MaxLength(50)]
        public string? DepositReference { get; set; }
        [MaxLength(50)]
        public string? TCReference { get; set; }
        public string? UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<SaleProduct>? SaleProducts { get; set; }
    }
}
