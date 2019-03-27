using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BillRepository: EfRepository<Bill, int>, IBillRepository
    {
        public BillRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
