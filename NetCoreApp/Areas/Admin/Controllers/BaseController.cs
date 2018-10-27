using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;

namespace NetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {
        public IServiceRegistration ServiceRegistration { get; set; }

        public BaseController()
        {
            if (ServiceRegistration == null)
            {
                ServiceRegistration = new ServiceRegistration();
            }
        }
    }
}