using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Models;
using NetCoreApp.Utilities.Constants;
using Newtonsoft.Json;

namespace NetCoreApp.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.GetString(CommonConstants.CartSession);
            var cart = new List<ShoppingCartViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(session);
            }

            return View(cart);
        }
    }
}
