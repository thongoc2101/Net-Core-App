using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BlogTagRepository: EfRepository<BlogTag, int>, IBlogTagRepository
    {
        private readonly AppDbContext _dbContext;
        public BlogTagRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
