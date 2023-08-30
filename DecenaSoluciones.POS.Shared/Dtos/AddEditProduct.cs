using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditProduct
    {
        public string? Code { get; set; }

        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        public string Description { get; set; } = string.Empty;
        public int? MaintenancePeriods { get; set; }
        public int? WarrantyTime { get; set; }
        public int stock { get; set; }

        [Required(ErrorMessage = "El campo Precio es requerido.")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "El precio debe ser mayor de 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo Costo es requerido.")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "El costo debe ser mayor de 0.")]
        public decimal Cost { get; set; }
        public bool Assignable { get; set; }
        public decimal ITBIS { get; set; } = 18.0M;

        public AddEditProduct()
        {
        }

        public AddEditProduct(ProductViewModel viewModel)
        {
            Code = viewModel.Code;
            Description = viewModel.Description;
            MaintenancePeriods = viewModel.MaintenancePeriods;
            WarrantyTime = viewModel.WarrantyTime;
            stock = viewModel.stock;
            Price = viewModel.Price;
            Cost = viewModel.Cost;
            Assignable = viewModel.Assignable;
            ITBIS = viewModel.ITBIS;
        }
    }
}
