using DecenaSoluciones.POS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class InventoryReportViewModel
    {
        public List<InventoryInReport> inventoryInEntries { get; set; } = new List<InventoryInReport>();
        public List<InventoryInDetailReport> inventoryInEntriesDetails { get; set; } = new List<InventoryInDetailReport>();
        public List<InventoryOutReport> inventoryOutEntries { get; set; } = new List<InventoryOutReport>();
        public List<InventoryOutDetailReport> inventoryOutEntriesDetails { get; set; } = new List<InventoryOutDetailReport>();
    }

    public class InventoryInReport
    {
        public string ProviderName { get; set; }
        public string CreationDate { get; set; }
        public string UserName { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ProductQuantity { get; set; }
    }

    public class InventoryOutReport
    {
        public string CreationDate { get; set; }
        public string UserName { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ProductQuantity { get; set; }
    }

    public class InventoryInDetailReport
    {
        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { 
            get { 
                return UnitCost * Quantity; 
            }
        }
    }

    public class InventoryOutDetailReport
    {
        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public string Comments { get; set; }
        public decimal TotalCost
        {
            get
            {
                return UnitCost * Quantity;
            }
        }
    }
}
