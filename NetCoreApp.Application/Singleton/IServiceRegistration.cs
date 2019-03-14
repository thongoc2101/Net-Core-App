using NetCoreApp.Application.Interfaces;

namespace NetCoreApp.Application.Singleton
{
    public interface IServiceRegistration
    {
        IProductService ProductService { get; }

        IProductCategoryService ProductCategoryService { get; }

        IFunctionService FunctionService { get; }

        IBillService BillService { get; }
    }
}
