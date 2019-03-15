using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ProductQuantityRepository: EfRepository<ProductQuantity, int>, IProductQuantityRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductQuantityRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
