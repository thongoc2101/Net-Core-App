using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.EF.Repositories;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Registration
{
    public interface IUnitOfWork
    {
        IProductCategoryRepository ProductCategoryRepository { get; }

        IFunctionRepository FunctionRepository { get; }

        IProductRepository ProductRepository { get; }

        ITagRepository TagRepository { get; }

        IProductTagRepository ProductTagRepository { get; }

        IPermissionRepository PermissionRepository { get; }

        IBillRepository BillRepository { get; }

        IBillDetailRepository BillDetailRepository { get; }

        IColorRepository ColorRepository { get; }

        ISizeRepository SizeRepository { get; }

        IProductQuantityRepository ProductQuantityRepository { get; }

        IProductImageRepository ProductImageRepository { get; }

        IWholePriceRepository WholePriceRepository { get; }

        IFooterRepository FooterRepository { get; }

        ISystemConfigRepository SystemConfigRepository { get; }

        ISlideRepository  SlideRepository { get; }

        IBlogRepository BlogRepository { get; }

        IBlogTagRepository BlogTagRepository { get; }

        void Commit();
    }
}
