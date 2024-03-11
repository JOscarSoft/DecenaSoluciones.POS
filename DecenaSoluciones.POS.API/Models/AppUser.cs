using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "AppUser")]
    public class AppUser : IdentityUser
    {
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
    }
}
