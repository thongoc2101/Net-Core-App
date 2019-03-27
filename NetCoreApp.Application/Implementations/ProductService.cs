using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Constants;
using NetCoreApp.Utilities.Dtos;
using NetCoreApp.Utilities.Helpers;
using OfficeOpenXml;

namespace NetCoreApp.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        var tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _unitOfWork.TagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = Mapper.Map<ProductViewModel, Product>(productVm);
                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
                _unitOfWork.ProductRepository.Add(product);

            }
            _unitOfWork.Commit();
            return productVm;
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            
            return _unitOfWork.ProductRepository.FindAll(x=>x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _unitOfWork.ProductRepository.FindAll();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);
            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paging = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paging;
        }

        public ProductViewModel GetProductById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_unitOfWork.ProductRepository.FindById(id));
        }

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        var tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _unitOfWork.TagRepository.Add(tag);
                    }
                    _unitOfWork.ProductTagRepository.RemoveMultiple(_unitOfWork.ProductTagRepository.FindAll(x => x.Id == productVm.Id).ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }

            // fix dateCreated
            var oldProduct = _unitOfWork.ProductRepository.FindAll(x => x.Id == productVm.Id).AsNoTracking().First();

            var product = Mapper.Map<ProductViewModel, Product>(productVm);

            if (oldProduct != null) product.DateCreated = oldProduct.DateCreated;

            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                var product = new Product();
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product.CategoryId = categoryId;

                    product.Name = workSheet.Cells[i, 1].Value.ToString();

                    product.Description = workSheet.Cells[i, 2].Value.ToString();

                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;

                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);

                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();

                    product.SeoDescription = workSheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);

                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    _unitOfWork.ProductRepository.Add(product);
                    
                }
            }
            _unitOfWork.Commit();
        }

        public void AddQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _unitOfWork.ProductQuantityRepository.RemoveMultiple(_unitOfWork.ProductQuantityRepository.FindAll(x=> x.ProductId==productId).ToList());
            foreach (var quantity in quantities)
            {
                _unitOfWork.ProductQuantityRepository.Add(new ProductQuantity()
                {
                    ProductId = productId,
                    ColorId = quantity.ColorId,
                    SizeId = quantity.SizeId,
                    Quantity = quantity.Quantity
                });
            }
            _unitOfWork.Commit();
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            return _unitOfWork.ProductQuantityRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<ProductQuantityViewModel>().ToList();
        }

        public void AddImages(int productId, string[] images)
        {
            _unitOfWork.ProductImageRepository.RemoveMultiple(_unitOfWork.ProductImageRepository
                .FindAll(x => x.ProductId == productId).ToList());

            foreach (var image in images)
            {
                _unitOfWork.ProductImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }
            _unitOfWork.Commit();
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return _unitOfWork.ProductImageRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<ProductImageViewModel>().ToList();
        }

        public void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _unitOfWork.WholePriceRepository.RemoveMultiple(_unitOfWork.WholePriceRepository
                .FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                _unitOfWork.WholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }

            _unitOfWork.Commit();
        }

        public List<WholePriceViewModel> GetWholePrices(int productId)
        {
            return _unitOfWork.WholePriceRepository.FindAll(x => x.ProductId == productId)
                .ProjectTo<WholePriceViewModel>().ToList();
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            return _unitOfWork.ProductRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            return _unitOfWork.ProductRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetRelatedProducts(int id, int top)
        {
            var product = _unitOfWork.ProductRepository.FindById(id);
            return _unitOfWork.ProductRepository.FindAll(x => x.Status == Status.Active && x.Id != id && x.CategoryId == product.CategoryId)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<ProductViewModel> GetUpSellProducts(int top)
        {
            return _unitOfWork.ProductRepository.FindAll(x => x.PromotionPrice != null)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<ProductViewModel>()
                .ToList();
        }

        public List<TagViewModel> GetTags(int productId)
        {
            var tags = _unitOfWork.TagRepository.FindAll();
            var productTags = _unitOfWork.ProductTagRepository.FindAll();

            var query = from t in tags
                join pt in productTags on t.Id equals pt.TagId
                where pt.ProductId == productId
                select new TagViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                };
            return query.ToList();
        }

        public bool CheckAvailable(int productId, int size, int color)
        {
            var checkAvailable = _unitOfWork.ProductQuantityRepository.FindSingle(x =>
                x.ColorId.Equals(color) && x.SizeId.Equals(size) && x.ProductId.Equals(productId));
            if (checkAvailable == null)
            {
                return false;
            }

            return checkAvailable.Quantity > 0;
        }
    }
}
