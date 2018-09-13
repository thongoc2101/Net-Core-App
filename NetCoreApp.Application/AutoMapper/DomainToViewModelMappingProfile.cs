using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Mapping from Model to View Model
        /// </summary>
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}
