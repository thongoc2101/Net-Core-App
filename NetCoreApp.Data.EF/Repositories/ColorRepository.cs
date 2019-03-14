using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ColorRepository: EfRepository<Color, int>, IColorRepository
    {
        private readonly AppDbContext _context;

        public ColorRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
