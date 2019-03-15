using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreApp.Application.Singleton;
using NetCoreApp.Models.ProductViewModels;

namespace NetCoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceRegistration _serviceRegistration;
        private readonly IConfiguration _configuration;

        public ProductController(IServiceRegistration serviceRegistration,
            IConfiguration configuration)
        {
            _serviceRegistration = serviceRegistration;
            _configuration = configuration;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";

            if (pageSize == null)
            {
                pageSize = _configuration.GetValue<int>("PageSize");
            }

            var catalogVm = new CatalogViewModel
            {
                Data = _serviceRegistration.ProductService.GetAllPaging(id, String.Empty, page, pageSize.Value),
                Category = _serviceRegistration.ProductCategoryService.GetById(id),
                PageSize = pageSize,
                SortType = sortBy
            };

            return View(catalogVm);
        }

        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = "shop_grid_full_width_page";

            if (pageSize == null)
            {
                pageSize = _configuration.GetValue<int>("PageSize");
            }

            var searchVm = new SearchResultViewModel()
            {
                Data = _serviceRegistration.ProductService.GetAllPaging(null, keyword, page, pageSize.Value),
                Keyword = keyword,
                PageSize = pageSize,
                SortType = sortBy
            };

            return View(searchVm);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";

            var details = new DetailViewModel
            {
                Category = _serviceRegistration.ProductCategoryService.GetById(id),
                Product = _serviceRegistration.ProductService.GetProductById(id),
                ProductImage = _serviceRegistration.ProductService.GetImages(id),
                RelatedProduct = _serviceRegistration.ProductService.GetRelatedProducts(id, 9),
                UpSellProduct = _serviceRegistration.ProductService.GetUpSellProducts(6),
                Tags = _serviceRegistration.ProductService.GetTags(id)
            };


            return View(details);
        }
    }
}