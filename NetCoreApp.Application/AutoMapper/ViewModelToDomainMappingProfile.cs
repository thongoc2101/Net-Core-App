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
                .ConstructUsing(c => new Product());
        }
    }
}
