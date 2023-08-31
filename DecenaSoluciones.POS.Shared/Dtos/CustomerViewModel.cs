using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Direction { get; set; }
        public string? Product { get; set; }
        public DateOnly? NextMaintenance { get; set; }
    }
}
