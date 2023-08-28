using System;
using System.Collections.Generic;
using System.Text;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? MaintenancePeriods { get; set; }
        public int? WarrantyTime { get; set; }
        public int stock { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public bool Assignable { get; set; }
        public decimal ITBIS { get; set; }
    }
}
