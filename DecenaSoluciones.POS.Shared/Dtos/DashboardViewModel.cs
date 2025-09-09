using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class DashboardViewModel
    {
        public string CompanyName { get; set; } = string.Empty;
        public int ExpiredMaintenances { get; set; } = 0;
        public int SoldProductsPerWeek { get; set; } = 0;
        public int ProductsWithEmptyStock { get; set; } = 0;

        public List<SoldProductQuantityViewModel> SoldProducts { get; set; } = new List<SoldProductQuantityViewModel>();
    }

    public class SoldProductQuantityViewModel
    {
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }

    }
}
