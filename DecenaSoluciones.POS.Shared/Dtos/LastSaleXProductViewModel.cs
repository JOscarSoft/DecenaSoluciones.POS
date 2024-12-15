using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class LastSaleXProductViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SalePrice { get; set; }
    }
}
