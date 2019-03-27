using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class SystemConfigRepository: EfRepository<SystemConfig, string>, ISystemConfigRepository
    {
        private readonly AppDbContext _dbContext;
        public SystemConfigRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
