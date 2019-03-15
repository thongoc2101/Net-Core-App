using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;
using NetCoreApp.Extensions;
using NetCoreApp.Models;

namespace NetCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRegistration _serviceRegistration;

        public HomeController(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }

        public IActionResult Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel()
            {
                HomeCategories = _serviceRegistration.ProductCategoryService.GetHomeCategories(5),
                HotProducts = _serviceRegistration.ProductService.GetHotProduct(5),
                TopSellProducts = _serviceRegistration.ProductService.GetLastest(5),
                LastestBlogs = _serviceRegistration.BlogService.GetLastest(5),
                HomeSlides = _serviceRegistration.CommonService.GetSlides("top")
            };

            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
