using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreApp.Data.EF.Repositories;
using NetCoreApp.Data.IRepositories;
using NetCoreApp.Utilities.Constants;

namespace NetCoreApp.Data.EF.Registration
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository _productRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IFunctionRepository _functionRepository;

        public UnitOfWork()
        {
            if (_context == null)
            {
                var configuration = new ConfigurationBuilder().SetBasePath(Directory
                    .GetCurrentDirectory()).AddJsonFile(CommonConstants.DefaultFileName).Build();
                _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(configuration.GetConnectionString(CommonConstants.DefaultConnection)).Options);
            }
        }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository =>
            _productRepository ?? (_productRepository = new ProductRepository(_context));

        public IProductCategoryRepository ProductCategoryRepository =>
            _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_context));

        public IFunctionRepository FunctionRepository =>
            _functionRepository ?? (_functionRepository = new FunctionRepository(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
