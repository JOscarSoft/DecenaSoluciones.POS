using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del contacto es requerido")]
        public string ContactName { get; set; }
        public string? ContactEmail { get; set; }

        [Required(ErrorMessage = "El telefono es requerido")]
        public string ContactPhone { get; set; }
        public DateTime SubscriptionExpiration { get; set; }
        public bool Active { get {  return SubscriptionExpiration > DateTime.Now; } }
        public string? Address { get; set; }
        public string? Slogan { get; set; }
        public string? QuotationsReceipt { get; set; }
        public string? SalesReceipt { get; set; }
    }
}
