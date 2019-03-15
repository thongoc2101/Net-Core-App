using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreApp.Data.Enums;
using NetCoreApp.Data.Interfaces;
using NetCoreApp.Infrastructure.SharedKernel;

namespace NetCoreApp.Data.Entities
{
    [Table("Products")]

    public class Product : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData
    {
        public Product()
        {
            ProductTags = new List<ProductTag>();
        }

        // dung cho add, khong can co id
        public Product(string name, int categoryId, string image, decimal price, decimal? promotionPrice,
            decimal originalPrice, string description, string content, bool? hotFlag, bool? homeFlag,
            int? viewCount, string tags, string unit, string seoAlias, string seoPageTitle, string seoDescription,
            string seoKeywords, DateTime dateCreated, DateTime dateModified, Status status)
        {
            Name = name;
            CategoryId = categoryId;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HotFlag = hotFlag;
            HomeFlag = homeFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            SeoAlias = seoAlias;
            SeoPageTitle = seoPageTitle;
            SeoDescription = seoDescription;
            SeoKeywords = seoKeywords;
            DateCreated = dateCreated;
            DateModified = dateModified;
            Status = status;
            ProductTags = new List<ProductTag>();
        }

        // DUng cho Update, can co id
        public Product( int id, string name, int categoryId, string image, decimal price, decimal? promotionPrice,
            decimal originalPrice, string description, string content, bool? hotFlag, bool? homeFlag,
            int? viewCount, string tags, string unit, string seoAlias, string seoPageTitle, string seoDescription,
            string seoKeywords, DateTime dateCreated, DateTime dateModified, Status status)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Image = image;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HotFlag = hotFlag;
            HomeFlag = homeFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            SeoAlias = seoAlias;
            SeoPageTitle = seoPageTitle;
            SeoDescription = seoDescription;
            SeoKeywords = seoKeywords;
            DateCreated = dateCreated;
            DateModified = dateModified;
            Status = status;
            ProductTags = new List<ProductTag>();
        }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string Unit { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { set; get; }

        public string SeoPageTitle {set;get;}

        [Column(TypeName ="varchar(255)")]
        [StringLength(255)]
        public string SeoAlias {set;get;}

        [StringLength(255)]
        public string SeoKeywords {set;get;}

        [StringLength(255)]
        public string SeoDescription {set;get;}

        public DateTime DateCreated {set;get;}
        public DateTime DateModified {set;get;}

        public Status Status {set;get;}

        public virtual ICollection<ProductTag> ProductTags { set; get; }
    }
}
