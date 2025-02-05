using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Companies")]
    public class Company : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? ContactName { get; set; }

        [MaxLength(50)]
        public string? ContactEmail { get; set; }

        [MaxLength(25)]
        public string? ContactPhone { get; set; }

        [MaxLength(225)]
        public string? Address { get; set; }

        [MaxLength(80)]
        public string? Slogan { get; set; }
        public DateTime SubscriptionExpiration { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? QuotationsReceipt { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SalesReceipt { get; set; }
    }
}
