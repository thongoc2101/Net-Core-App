using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using NetCoreApp.Application.Implementations;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Application.Singleton
{
    public class ServiceRegistration : IServiceRegistration
    {
        private IProductCategoryService _productCategoryService;
        private FunctionService _functionService;
        private IProductService _productService;
        private IBillService _billService;
        private IBlogService _blogService;
        private ICommonService _commonService;

        private readonly IUnitOfWork _unitOfWork;

        public ServiceRegistration()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IProductCategoryService ProductCategoryService =>
            _productCategoryService ?? (_productCategoryService = new ProductCategoryService(_unitOfWork));

        public IFunctionService FunctionService =>
            _functionService ?? (_functionService = new FunctionService(_unitOfWork));

        public IProductService ProductService => _productService ?? (_productService = new ProductService(_unitOfWork));

        public IBillService BillService => _billService ?? (_billService = new BillService(_unitOfWork));

        public IBlogService BlogService => _blogService ?? (_blogService = new BlogService(_unitOfWork));

        public ICommonService CommonService => _commonService ?? (_commonService = new CommonService(_unitOfWork));

    }
}
