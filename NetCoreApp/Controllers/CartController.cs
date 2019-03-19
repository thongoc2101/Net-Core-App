using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Application.Singleton;
using NetCoreApp.Extensions;
using NetCoreApp.Models;
using NetCoreApp.Utilities.Constants;

namespace NetCoreApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IServiceRegistration _serviceRegistration;

        public CartController(IServiceRegistration serviceRegistration)
        {
            _serviceRegistration = serviceRegistration;
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("checkout.html", Name = "Checkout")]
        public IActionResult CheckOut()
        {
            return View();
        }

        #region AJAX Request
        /// <summary>
        /// Get list cart
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession) ?? new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Remove all product in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult CleanCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// add cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="colorId"></param>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        public IActionResult AddCart(int productId, int quantity, int colorId, int sizeId)
        {
            //Get product detail
            var product = _serviceRegistration.ProductService.GetProductById(productId);

            // Get session with item list for cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //convert string to list object
                bool hasChanged = false;

                // check exist with item product id
                if (session.Any(x => x.Product.Id.Equals(productId)))
                {
                    // Update quantity for product if match product id
                    foreach (var item in session)
                    {
                        if (item.Product.Id.Equals(productId))
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = _serviceRegistration.BillService.GetColors(colorId),
                        Size = _serviceRegistration.BillService.GetSizes(sizeId),
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }

                // Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                // Add new cart
                var cart = new List<ShoppingCartViewModel>
                {
                    new ShoppingCartViewModel
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.PromotionPrice ?? product.Price,
                        Color = _serviceRegistration.BillService.GetColors(colorId),
                        Size = _serviceRegistration.BillService.GetSizes(sizeId),
                    }
                };
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }

            return new OkObjectResult(productId);
        }

        /// <summary>
        /// remove for product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveForProduct(int productId)
        {
            // Get session with item list for cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //convert string to list object
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id.Equals(productId))
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }

                // Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }

                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update cart for product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            //Get session with item list for cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //convert string to list object
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id.Equals(productId))
                    {
                        var product = _serviceRegistration.ProductService.GetProductById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Color = _serviceRegistration.BillService.GetColors(color);
                        item.Size = _serviceRegistration.BillService.GetSizes(size);
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }

                // Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }

                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _serviceRegistration.BillService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _serviceRegistration.BillService.GetSizes();
            return new OkObjectResult(sizes);
        }
        #endregion
    }
}