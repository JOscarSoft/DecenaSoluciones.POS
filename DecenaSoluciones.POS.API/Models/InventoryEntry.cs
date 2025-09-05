using DecenaSoluciones.POS.API.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "InventoryEntries")]
    public class InventoryEntry : BaseEntity, ICompanyEntity
    {
        public int? ProviderId { get; set; }
        public string? UserName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }
        public required int InventoryEntryTypeId { get; set; }
        public virtual InventoryEntryType? InventoryEntryType { get; set; }
        public virtual Company Company { get; set; }
        public virtual Provider? Provider { get; set; }
        public virtual ICollection<InventoryEntryDetail>? InventoryEntryDetails { get; set; }
    }
}
