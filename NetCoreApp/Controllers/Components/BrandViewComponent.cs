using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Controllers.Components
{
    public class BrandViewComponent: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
