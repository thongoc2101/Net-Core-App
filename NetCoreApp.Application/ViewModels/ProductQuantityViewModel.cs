using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApp.Application.ViewModels
{
    public class ProductQuantityViewModel
    {
        public int Id { get; set; }

        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Column(Order = 2)]
        public int SizeId { get; set; }


        [Column(Order = 3)]
        public int ColorId { get; set; }

        public int Quantity { get; set; }
        
        public ProductViewModel Product { get; set; }

        public SizeViewModel Size { get; set; }

        public ColorViewModel Color { get; set; }
    }
}
