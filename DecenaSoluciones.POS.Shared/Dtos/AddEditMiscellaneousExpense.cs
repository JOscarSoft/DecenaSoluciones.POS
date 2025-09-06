using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class AddEditMiscellaneousExpense
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "El comentario es requerido.")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "El gasto es requerido.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El gasto debe ser mayor de 0.")]
        public decimal TotalCost { get; set; }
    }
}
