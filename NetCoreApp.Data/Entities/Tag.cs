using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCoreApp.Infrastructure.SharedKernel;

namespace NetCoreApp.Data.Entities
{
    [Table("Tags")]
    public class Tag : DomainEntity<string>
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
