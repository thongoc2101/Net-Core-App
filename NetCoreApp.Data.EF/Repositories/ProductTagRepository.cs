using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ProductTagRepository : EfRepository<ProductTag, int>, IProductTagRepository
    {
        public ProductTagRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
