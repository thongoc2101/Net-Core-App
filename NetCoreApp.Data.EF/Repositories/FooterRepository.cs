using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class FooterRepository: EfRepository<Footer, string>, IFooterRepository
    {
        private readonly AppDbContext _dbContext;
        public FooterRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
