using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class WholePriceRepository: EfRepository<WholePrice, int>, IWholePriceRepository
    {
        private readonly AppDbContext _dbContext;
        public WholePriceRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
