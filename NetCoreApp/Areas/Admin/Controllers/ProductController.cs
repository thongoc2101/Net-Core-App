using Microsoft.AspNetCore.Mvc;

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

        #endregion
    }
}