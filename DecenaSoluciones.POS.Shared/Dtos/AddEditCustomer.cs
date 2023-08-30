using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditCustomer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es requerido.")]
        public required string Name { get; set; }
        public string? LastName { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Direction { get; set; }
        public ICollection<AddEditCustomerProduct>? CustomerProducts { get; set; }
    }

    public class AddEditCustomerProduct
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime? LastMaintenance { get; set; } = DateTime.Now;
        public DateTime? NextMaintenance { get; set; } = DateTime.Now.AddMonths(6);
        public DateTime? WarrantyEndDate { get; set; }
        public bool SoldByUs { get; set; } = true;
        public bool HasWarranty { get; set; } = false;
        public bool NeedMaintenance { get; set; } = true;
        public string? Serial { get; set; }
        public DateTime? SaleDate { get; set; }
        public bool Active { get; set; } = true;
    }
}
