﻿using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class SalesReportViewModel
    {
        public string? Code { get; set; }
        public string? CustomerName { get; set; }
        public int ProductsQuantity { get; set; }
        public decimal? PayedAmount { get; set; }
        public decimal Devolution { get
            {
                return (PayedAmount ?? 0m) - (Total ?? 0m);
            }
        }
        public decimal? Total { get; set; }
        public string? UserName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
