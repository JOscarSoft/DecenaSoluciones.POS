using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "QuotationProducts")]
    public class QuotationProduct : BaseEntity, ICompanyEntity
    {
        public int QuotationId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ITBIS { get; set; }
        public string? Comments { get; set; }
        public decimal Total { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual required Quotation Quotation { get; set; }

        public virtual required Product Product { get; set; }
    }
}
