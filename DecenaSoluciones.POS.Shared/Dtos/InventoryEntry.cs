using DecenaSoluciones.POS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class InventoryEntryViewModel
    {
        public int Id { get; set; }
        public int? ProviderId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public InventoryEntryType InventoryEntryType { get; set; }
        public string? UserName { get; set; }
        public decimal? TotalCost { get; set; }
        public ICollection<InventoryEntryDetailViewModel>? Details { get; set; } = new List<InventoryEntryDetailViewModel>();
    }

    public class InventoryEntryDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto es requerido")]
        public string? ProductCode { get; set; }

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "El campo Precio es requerido.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El precio debe ser mayor de 0.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El campo Costo es requerido.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El costo debe ser mayor de 0.")]
        public decimal UnitCost { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es requerido.")]
        public decimal Quantity { get; set; }
        public decimal TotalCost { 
            get { 
                return UnitCost * Quantity; 
            }
        }
    }
}
