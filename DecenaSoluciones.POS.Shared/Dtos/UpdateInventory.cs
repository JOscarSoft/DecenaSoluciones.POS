using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class UpdateInventory
    {
        [Required(ErrorMessage = "El producto a actualizar es requerido")]
        public string? ProductCode { get; set; }

        [Required(ErrorMessage = "El campo Precio es requerido.")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "El precio debe ser mayor de 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo Costo es requerido.")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "El costo debe ser mayor de 0.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es requerido.")]
        public int Quantity { get; set; }
    }
}
