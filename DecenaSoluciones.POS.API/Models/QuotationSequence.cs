using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "QuotationSequence")]
    public class QuotationSequence : BaseEntity, ICompanyEntity
    {
        [MaxLength(25)]
        public required string Code { get; set; }
        public int Sequence { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
