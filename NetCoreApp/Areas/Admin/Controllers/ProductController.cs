using System;
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
using OfficeOpenXml;
using OfficeOpenXml.Table;

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
        /// <summary>
        /// Get all product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return new OkObjectResult(ServiceRegistration.ProductService.GetAll());
        }


        /// <summary>
        /// Get all product category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return new OkObjectResult(ServiceRegistration.ProductCategoryService.GetAll());
        }


        /// <summary>
        /// Get all product has paging
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
                productViewModel.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
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
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

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

        [HttpPost]
        public IActionResult ExportExcel()
        {
            // create folder "export-files"
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            // check folder exists??? 
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // create file name
            string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            // check file exists
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            // query get all product(co the thay doi tuy theo yeu cau)
            var products = ServiceRegistration.ProductService.GetAll();
            // get tung dong vao file
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        #region Product Quantities

        [HttpPost]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            ServiceRegistration.ProductService.AddQuantities(productId, quantities);
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantity = ServiceRegistration.ProductService.GetQuantities(productId);
            return new ObjectResult(quantity);
        }

        #endregion

        #region Upload Images

        [HttpPost]
        public IActionResult SaveImages(int productId, string[] images)
        {
            ServiceRegistration.ProductService.AddImages(productId, images);
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = ServiceRegistration.ProductService.GetImages(productId);
            return new ObjectResult(images);
        }

        #endregion

        #region whole price management

        [HttpPost]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            ServiceRegistration.ProductService.AddWholePrice(productId, wholePrices);
            return new OkObjectResult(wholePrices);
        }

        [HttpGet]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrice = ServiceRegistration.ProductService.GetWholePrices(productId);
            return new ObjectResult(wholePrice);
        }

        #endregion
        #endregion
    }
}