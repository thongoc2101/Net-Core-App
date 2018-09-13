using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ProductCategoryRepository : EfRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private AppDbContext _dbContext;
        public ProductCategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _dbContext.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}
