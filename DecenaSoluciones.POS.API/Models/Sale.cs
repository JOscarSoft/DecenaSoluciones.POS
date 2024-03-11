using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Sales")]
    public class Sale : BaseEntity, ICompanyEntity
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
        public bool? CreditSale { get; set; }
        public DateTime CreationDate { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<SaleProduct>? SaleProducts { get; set; }
    }
}
