using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Interfaces;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api
        [HttpGet]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_productService.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return new OkObjectResult(_productCategoryService.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            return new OkObjectResult(_productService.GetAllPaging(categoryId, keyword, page, pageSize));
        }

        #endregion
    }
}