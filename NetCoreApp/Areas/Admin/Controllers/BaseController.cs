using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;
using NetCoreApp.Data.Entities;

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