using System.ComponentModel.DataAnnotations;

namespace DecenaSoluciones.POS.API.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
