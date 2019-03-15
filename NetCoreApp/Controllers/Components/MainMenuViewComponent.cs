using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;

namespace NetCoreApp.Controllers.Components
{

    public class MainMenuViewComponent: ViewComponent
    {
        private readonly IServiceRegistration _serviceRegistration;

        public MainMenuViewComponent(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_serviceRegistration.ProductCategoryService.GetAll());
        }
    }
}
