using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BlogRepository: EfRepository<Blog, int>, IBlogRepository
    {
        private readonly AppDbContext _dbContext;
        public BlogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
