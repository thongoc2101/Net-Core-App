using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class SlideRepository: EfRepository<Slide, int>, ISlideRepository
    {
        private readonly AppDbContext _dbContext;
        public SlideRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
