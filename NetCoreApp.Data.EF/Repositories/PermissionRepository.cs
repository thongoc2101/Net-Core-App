using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class PermissionRepository: EfRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
