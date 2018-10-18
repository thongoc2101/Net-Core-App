using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ProductRepository : EfRepository<Product,int>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
