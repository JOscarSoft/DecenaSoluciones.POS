using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class SalesViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal? Total { get; set; }
        public string? CustomerName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAQuotation { get; set; }
    }
}
