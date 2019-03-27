using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ProductImageRepository: EfRepository<ProductImage, int>, IProductImageRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductImageRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
