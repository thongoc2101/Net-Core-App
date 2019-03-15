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
        private IProductCategoryRepository _productCategoryRepository;
        private IFunctionRepository _functionRepository;
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IPermissionRepository _permissionRepository;
        private IBillRepository _billRepository;
        private IBillDetailRepository _billDetailRepository;
        private IColorRepository _colorRepository;
        private ISizeRepository _sizeRepository;
        private IProductQuantityRepository _productQuantityRepository;
        private IProductImageRepository _productImageRepository;
        private IWholePriceRepository _wholePriceRepository;
        private IFooterRepository _footerRepository;
        private ISystemConfigRepository _systemConfigRepository;
        private ISlideRepository _slideRepository;
        private IBlogRepository _blogRepository;
        private IBlogTagRepository _blogTagRepository;



        public UnitOfWork()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(CommonConstants.DefaultJsonFile)
                .Build();
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(configuration.GetConnectionString(CommonConstants.DefaultConnection)).Options);
        }

        public IProductCategoryRepository ProductCategoryRepository =>
            _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_context));

        public IFunctionRepository FunctionRepository =>
            _functionRepository ?? (_functionRepository = new FunctionRepository(_context));

        public IProductRepository ProductRepository =>
            _productRepository ?? (_productRepository = new ProductRepository(_context));

        public ITagRepository TagRepository => _tagRepository ?? (_tagRepository = new TagRepository(_context));

        public IProductTagRepository ProductTagRepository =>
            _productTagRepository ?? (_productTagRepository = new ProductTagRepository(_context));

        public IPermissionRepository PermissionRepository =>
            _permissionRepository ?? (_permissionRepository = new PermissionRepository(_context));

        public IBillRepository BillRepository => _billRepository ?? (_billRepository = new BillRepository(_context));

        public IBillDetailRepository BillDetailRepository =>
            _billDetailRepository ?? (_billDetailRepository = new BillDetailRepository(_context));

        public IColorRepository ColorRepository =>
            _colorRepository ?? (_colorRepository = new ColorRepository(_context));

        public ISizeRepository SizeRepository => _sizeRepository ?? (_sizeRepository = new SizeRepository(_context));

        public IProductQuantityRepository ProductQuantityRepository =>
            _productQuantityRepository ?? (_productQuantityRepository = new ProductQuantityRepository(_context));

        public IProductImageRepository ProductImageRepository =>
            _productImageRepository ?? (_productImageRepository = new ProductImageRepository(_context));

        public IWholePriceRepository WholePriceRepository =>
            _wholePriceRepository ?? (_wholePriceRepository = new WholePriceRepository(_context));

        public IFooterRepository FooterRepository =>
            _footerRepository ?? (_footerRepository = new FooterRepository(_context));

        public ISystemConfigRepository SystemConfigRepository =>
            _systemConfigRepository ?? (_systemConfigRepository = new SystemConfigRepository(_context));

        public ISlideRepository SlideRepository =>
            _slideRepository ?? (_slideRepository = new SlideRepository(_context));

        public IBlogRepository BlogRepository => _blogRepository ?? (_blogRepository = new BlogRepository(_context));

        public IBlogTagRepository BlogTagRepository =>
            _blogTagRepository ?? (_blogTagRepository = new BlogTagRepository(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
