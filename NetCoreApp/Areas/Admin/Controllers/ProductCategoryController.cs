using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Helpers;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax Api

        /// <summary>
        /// Get product category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = ServiceRegistration.ProductCategoryService.GetById(id);
            return new OkObjectResult(model);
        }


        /// <summary>
        /// Update product category, save change
        /// </summary>
        /// <param name="productCategoryViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productCategoryViewModel.SeoAlias = TextHelper.ToUnsignString(productCategoryViewModel.Name);
                if (productCategoryViewModel.Id == 0)
                {
                    ServiceRegistration.ProductCategoryService.Add(productCategoryViewModel);
                }
                else
                {
                    ServiceRegistration.ProductCategoryService.Update(productCategoryViewModel);
                }

                return new OkObjectResult(productCategoryViewModel);
            }
        }


        /// <summary>
        /// Delete product category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                ServiceRegistration.ProductCategoryService.Delete(id);
                return new OkResult();
            }
        }


        /// <summary>
        /// Get all product category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = ServiceRegistration.ProductCategoryService.GetAll();
            return new OkObjectResult(model);
        }


        /// <summary>
        /// update parent id, paging
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    ServiceRegistration.ProductCategoryService.UpdateParentId(sourceId, targetId, items);

                    return new OkResult();
                }
            }
        }


        /// <summary>
        /// reorder
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    ServiceRegistration.ProductCategoryService.ReOrder(sourceId, targetId);

                    return new OkResult();
                }
            }
        }

        #endregion
    }
}