using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Mapping from View Model to Model through constructor
        /// </summary>
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image,
                c.HomeFlag, c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.PromotionPrice,c.OriginalPrice, c.Description, c.Content,
                    c.HotFlag, c.HomeFlag, c.ViewCount, c.Tags, c.Unit, c.SeoAlias, c.SeoPageTitle, c.SeoDescription, c.SeoKeywords,
                    c.DateCreated, c.DateModified, c.Status));
        }
    }
}
