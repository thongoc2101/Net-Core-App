using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return new OkObjectResult(roles);
        }
    }
}