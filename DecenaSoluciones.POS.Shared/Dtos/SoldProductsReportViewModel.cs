using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class SoldProductsReportViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal Stock { get; set; }
    }
}
