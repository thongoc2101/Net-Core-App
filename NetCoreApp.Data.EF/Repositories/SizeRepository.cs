using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class SizeRepository: EfRepository<Size, int>, ISizeRepository
    {
        private readonly AppDbContext _dbContext;
        public SizeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
