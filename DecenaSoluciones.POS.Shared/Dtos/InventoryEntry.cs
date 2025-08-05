using DecenaSoluciones.POS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class InventoryEntryViewModel
    {
        public int Id { get; set; }
        public int? ProviderId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public InventoryEntryType InventoryEntryType { get; set; }
        public string? UserName { get; set; }
        public ICollection<InventoryEntryDetailViewModel>? Details { get; set; } = new List<InventoryEntryDetailViewModel>();
    }

    public class InventoryEntryDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto a actualizar es requerido")]
        public string? ProductCode { get; set; }

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "El campo Precio es requerido.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El precio debe ser mayor de 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo Costo es requerido.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El costo debe ser mayor de 0.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es requerido.")]
        public decimal Quantity { get; set; }
    }
}
