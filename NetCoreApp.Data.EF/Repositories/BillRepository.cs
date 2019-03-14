using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BillRepository: EfRepository<Bill, int>, IBillRepository
    {
        private AppDbContext _context;
        public BillRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
