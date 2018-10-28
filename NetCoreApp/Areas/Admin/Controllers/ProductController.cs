using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Helpers;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api
        [HttpGet]
        public IActionResult GetAll()
        {
            return new OkObjectResult(ServiceRegistration.ProductService.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return new OkObjectResult(ServiceRegistration.ProductCategoryService.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            return new OkObjectResult(ServiceRegistration.ProductService.GetAllPaging(categoryId, keyword, page, pageSize));
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductById(int id)
        {
            var model = ServiceRegistration.ProductService.GetProductById(id);
            return new OkObjectResult(model);
        }


        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                ServiceRegistration.ProductService.Delete(id);
                return new OkResult();
            }
        }

        /// <summary>
        /// Update product, save change
        /// </summary>
        /// <param name="productViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productViewModel.SeoAlias = TextHelper.ToUnSignString(productViewModel.Name);
                if (productViewModel.Id == 0)
                {
                    ServiceRegistration.ProductService.Add(productViewModel);
                }
                else
                {
                    ServiceRegistration.ProductService.Update(productViewModel);
                }

                return new OkObjectResult(productViewModel);
            }
        }

        #endregion
    }
}