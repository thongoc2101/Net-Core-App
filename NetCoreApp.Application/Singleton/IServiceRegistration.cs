using NetCoreApp.Application.Interfaces;

namespace NetCoreApp.Application.Singleton
{
    public interface IServiceRegistration
    {
        IProductCategoryService ProductCategoryService { get; }

        IFunctionService FunctionService { get; }

        IProductService ProductService { get; }

        IBillService BillService { get; }

        IBlogService BlogService { get; }

        ICommonService CommonService { get; }
    }
}
