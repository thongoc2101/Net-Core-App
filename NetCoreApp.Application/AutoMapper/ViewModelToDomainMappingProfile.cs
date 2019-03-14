using System;
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
            CreateMap<AppUserViewModel, AppUser>()
                .ConstructUsing(c =>
                    new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName, c.Email, c.PhoneNumber, c.Avatar, c.Status));
            CreateMap<PermissionViewModel, Permission>()
                .ConstructUsing(c =>
                    new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanDelete, c.CanRead, c.CanUpdate));
            CreateMap<BillViewModel, Bill>()
                .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress, c.CustomerMobile,
                    c.CustomerMessage, c.BillStatus, c.PaymentMethod, c.Status, c.CustomerId));
            CreateMap<BillDetailViewModel, BillDetail>()
                .ConstructUsing(c =>
                    new BillDetail(c.Id, c.ProductId, c.Quantity, c.Price, c.ColorId, c.SizeId));
            //CreateMap<ProductQuantityViewModel, ProductQuantity>()
            //    .ConstructUsing(c => new ProductQuantity(c.Id, c.ProductId, c.SizeId, c.ColorId, c.Quantity));
            //CreateMap<ProductImageViewModel, ProductImage>()
            //    .ConstructUsing(c => new ProductImage(c.Id, c.ProductId, c.Path, c.Caption));
            //CreateMap<WholePriceViewModel, WholePrice>()
            //    .ConstructUsing(c => new WholePrice(c.Id, c.ProductId, c.FromQuantity, c.ToQuantity, c.Price));
        }
    }
}
