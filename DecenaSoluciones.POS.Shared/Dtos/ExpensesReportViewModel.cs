using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class ExpensesReportViewModel
    {
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Comments { get; set; }
        public decimal TotalCost { get; set; }
    }
}
