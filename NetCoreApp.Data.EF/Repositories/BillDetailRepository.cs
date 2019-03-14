using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BillDetailRepository: EfRepository<BillDetail, int>, IBillDetailRepository
    {
        private readonly AppDbContext _context;
        public BillDetailRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
