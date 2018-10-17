using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCoreApp.Data.Enums;
using NetCoreApp.Data.Interfaces;
using NetCoreApp.Infrastructure.SharedKernel;

namespace NetCoreApp.Data.Entities
{
    [Table("Advertisements")]
    public class Advertisement : DomainEntity<int>, ISwitchable, ISortable
    {
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(20)]
        public string PositionId { get; set; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public int SortOrder { set; get; }

        [ForeignKey("PositionId")]
        public virtual AdvertisementPosition AdvertisementPosition { get; set; }
    }
}
