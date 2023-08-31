using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditSale
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? CustomerId { get; set; }
        public decimal? WorkForceValue { get; set; }
        public decimal? Discount { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? DepositsAmount { get; set; }
        public decimal? TCAmount { get; set; }
        [MaxLength(50)]
        public string? DepositReference { get; set; }
        [MaxLength(50)]
        public string? TCReference { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAQuotation { get; set; } = false;
        public AddEditCustomer? Customer { get; set; } = new AddEditCustomer();
        public ICollection<AddEditSaleProduct>? SaleProducts { get; set; } = new List<AddEditSaleProduct>();
    }

    public class AddEditSaleProduct
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor de 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido.")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "El precio debe ser mayor de 0.")]
        public decimal UnitPrice { get; set; }
        public decimal ITBIS { get; set; }
        public string? Comments { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
