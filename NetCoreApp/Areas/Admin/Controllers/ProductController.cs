using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Helpers;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                ServiceRegistration.ProductService.ImportExcel(filePath, categoryId);

                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        #endregion
    }
}