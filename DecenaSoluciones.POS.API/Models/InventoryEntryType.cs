using DecenaSoluciones.POS.API.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "InventoryEntryTypes")]
    public class InventoryEntryType : BaseEntity
    {
        public string Description { get; set; }
    }
}
