using NetCoreApp.Application.Implementations;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Data.EF.Registration;

namespace NetCoreApp.Application.Singleton
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly IUnitOfWork _unitOfWork;
        private  IProductService _productService;
        private  IProductCategoryService _productCategoryService;
        private  IFunctionService _functionService;
        private IBillService _billService;

        public ServiceRegistration()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ServiceRegistration(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IProductService ProductService => _productService ?? (_productService = new ProductService(_unitOfWork));

        public IProductCategoryService ProductCategoryService =>
            _productCategoryService ?? (_productCategoryService = new ProductCategoryService(_unitOfWork));

        public IFunctionService FunctionService =>
            _functionService ?? (_functionService = new FunctionService(_unitOfWork));

        public IBillService BillService => _billService ?? (_billService = new BillService(_unitOfWork));
    }
}
