using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "SaleSequence")]
    public class SaleSequence : BaseEntity
    {
        public required string Code { get; set; }
        public int Sequence { get; set; }
    }
}
