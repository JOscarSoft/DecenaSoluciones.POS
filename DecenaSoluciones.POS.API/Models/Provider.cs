using DecenaSoluciones.POS.API.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecenaSoluciones.POS.API.Models
{
    [Table(name: "Providers")]
    public class Provider : BaseEntity, ICompanyEntity
    {
        public string? RNC { get; set; }

        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Direction { get; set; }

        public int CompanyId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public virtual Company Company { get; set; }
    }
}
