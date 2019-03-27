using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class BillDetailRepository: EfRepository<BillDetail, int>, IBillDetailRepository
    {
        public BillDetailRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
