using NetCoreApp.Data.EF.Repositories;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Registration
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        IProductCategoryRepository ProductCategoryRepository { get; }

        IFunctionRepository FunctionRepository { get; }

        ITagRepository TagRepository { get; }

        IProductTagRepository ProductTagRepository { get; }

        IPermissionRepository PermissionRepository { get; }

        IBillRepository BillRepository { get; }

        IBillDetailRepository BillDetailRepository { get; }

        ISizeRepository SizeRepository { get; }

        IColorRepository ColorRepository { get; }

        void Commit();
    }
}
