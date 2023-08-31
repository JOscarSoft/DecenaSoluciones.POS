﻿using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditCustomer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es requerido.")]
        public string Name { get; set; }
        public string? LastName { get; set; }
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Teléfono Incorrecto.")]
        public string? PhoneNumber { get; set; }
        public string? Direction { get; set; }
        public ICollection<AddEditCustomerProduct>? CustomerProducts { get; set; } = new List<AddEditCustomerProduct>();
    }

    public class AddEditCustomerProduct
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime? LastMaintenance { get; set; } = DateTime.Now;
        public DateTime? NextMaintenance { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public bool SoldByUs { get; set; } = true;
        public bool HasWarranty { get; set; } = false;
        public bool NeedMaintenance { get; set; } = true;
        public string? Serial { get; set; }
        public DateTime? SaleDate { get; set; } = DateTime.Now;
    }
}
