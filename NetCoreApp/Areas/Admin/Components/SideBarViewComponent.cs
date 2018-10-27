using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Extensions;
using NetCoreApp.Utilities.Constants;

namespace NetCoreApp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IServiceRegistration _serviceRegistration;

        public SideBarViewComponent(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal) User).GetSpecificClaim("Roles");
            List<FunctionViewModel> function;
            if (roles.Split(";").Contains(CommonConstants.AdminRole))
            {
                function = await _serviceRegistration.FunctionService.GetAll();
            }
            else
            {
                function = new List<FunctionViewModel>();
            }
            return View(function);
        }
    }
}
