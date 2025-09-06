using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "MiscellaneousExpenses")]
    public class MiscellaneousExpense : BaseEntity, ICompanyEntity
    {
        public required string UserName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public required string Comments { get; set; }
        public decimal TotalCost { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
