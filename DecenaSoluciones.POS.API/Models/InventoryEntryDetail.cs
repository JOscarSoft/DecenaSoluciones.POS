using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "InventoryEntryDetails")]
    public class InventoryEntryDetail : BaseEntity, ICompanyEntity
    {
        public required int InventoryEntryId { get; set; }
        public required int ProductId { get; set; }
        public required decimal Quantity { get; set; }
        public required decimal UnitCost { get; set; }
        public required decimal UnitPrice { get; set; }
        public string? Comments { get; set; }
        public decimal TotalCost { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual required InventoryEntry InventoryEntry { get; set; }
        public virtual required Product Product { get; set; }
    }
}
