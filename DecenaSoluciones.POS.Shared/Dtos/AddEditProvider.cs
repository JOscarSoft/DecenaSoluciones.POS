using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditProvider
    {
        public int Id { get; set; }
        public string? RNC { get; set; }
        public string? Name { get; set; }

        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Teléfono Incorrecto.")]
        public string? PhoneNumber { get; set; }
        public string? Direction { get; set; }
    }
}
