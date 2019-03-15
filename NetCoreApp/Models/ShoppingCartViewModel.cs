using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Models
{
    public class ShoppingCartViewModel
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
