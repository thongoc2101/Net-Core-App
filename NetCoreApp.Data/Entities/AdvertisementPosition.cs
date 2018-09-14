using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreApp.Infrastructure.SharedKernel;

namespace NetCoreApp.Data.Entities
{
    [Table("AdvertisementPositions")]
    public class AdvertisementPosition : DomainEntity<string>
    {
        [StringLength(20)]
        public string PageId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
        public virtual AdvertisementPage AdvertisementPage { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
