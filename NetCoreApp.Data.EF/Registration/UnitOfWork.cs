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
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IPermissionRepository _permissionRepository;
        private IBillRepository _billRepository;
        private IBillDetailRepository _billDetailRepository;
        private ISizeRepository _sizeRepository;
        private IColorRepository _colorRepository;

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

        public ITagRepository TagRepository => _tagRepository ?? (_tagRepository = new TagRepository(_context));

        public IProductTagRepository ProductTagRepository =>
            _productTagRepository ?? (_productTagRepository = new ProductTagRepository(_context));

        public IPermissionRepository PermissionRepository =>
            _permissionRepository ?? (_permissionRepository = new PermissionRepository(_context));

        public IBillRepository BillRepository =>
            _billRepository ?? (_billRepository = new BillRepository(_context));

        public IBillDetailRepository BillDetailRepository =>
            _billDetailRepository ?? (_billDetailRepository = new BillDetailRepository(_context));

        public ISizeRepository SizeRepository => _sizeRepository ?? (_sizeRepository = new SizeRepository(_context));

        public IColorRepository ColorRepository =>
            _colorRepository ?? (_colorRepository = new ColorRepository(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
