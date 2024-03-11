using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Companies")]
    public class Company : BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
