using System.ComponentModel.DataAnnotations.Schema;
using NetCoreApp.Infrastructure.SharedKernel;

namespace NetCoreApp.Data.Entities
{
    [Table("WholePrices")]
    public class WholePrice : DomainEntity<int>
    {
        public WholePrice(){ }

        public WholePrice(int id, int productId, int fromQuantity, int toQuantity, decimal price)
        {
            Id = id;
            ProductId = productId;
            FromQuantity = fromQuantity;
            ToQuantity = toQuantity;
            Price = price;
        }

        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
