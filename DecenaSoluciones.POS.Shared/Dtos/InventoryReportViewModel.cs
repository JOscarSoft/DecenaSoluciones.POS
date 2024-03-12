using System;
using System.Collections.Generic;
using System.Text;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class InventoryReportViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int stock { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal TotalCost
        {
            get
            {
                return stock * Cost;
            }
        }
        public decimal TotalPrice
        {
            get
            {
                return stock * Price;
            }
        }
        public decimal Revenue { 
            get
            {
                return TotalPrice - TotalCost;
            }
        }
    }
}
