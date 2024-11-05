using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditCompany
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del contacto es requerido")]
        public string ContactName { get; set; }
        public string? ContactEmail { get; set; }
        [Required(ErrorMessage = "El teléfono del contacto es requerido")]
        public string ContactPhone { get; set; }
        public DateTime SubscriptionExpiration { get; set; } = DateTime.Now;
        public string? Address { get; set; }
        public string? Slogan { get; set; }

        public AddEditCompany()
        {
            
        }

        public AddEditCompany(CompanyViewModel? companyViewModel)
        {
            if(companyViewModel == null)
                return;

            Name = companyViewModel.Name;
            ContactName = companyViewModel.ContactName;
            ContactEmail = companyViewModel.ContactEmail;
            ContactPhone = companyViewModel.ContactPhone;
            SubscriptionExpiration = companyViewModel.SubscriptionExpiration;
            Address = companyViewModel.Address;
            Slogan = companyViewModel.Slogan;
        }
    }
}
