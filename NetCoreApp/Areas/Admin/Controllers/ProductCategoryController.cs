using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Interfaces;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax Api
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        #endregion
    }
}