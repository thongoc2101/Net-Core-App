using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {

    }
}