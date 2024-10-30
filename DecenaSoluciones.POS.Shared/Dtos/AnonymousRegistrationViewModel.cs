using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AnonymousRegistrationViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El nombre del usuario es requerido")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "El nombre de la compañía es requerido")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "El telefono es requerido")]
        public string ContactPhone { get; set; }
        public int? CompanyId { get; set; }
    }
}
