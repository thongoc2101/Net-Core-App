using System;
using System.Collections.Generic;
using System.Deployment.Application;
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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _unitOfWork.ProductRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
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

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnSignString(t);
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

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnSignString(t);
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

            var oldProduct = _unitOfWork.ProductRepository.FindAll(x=>x.Id == productVm.Id).AsNoTracking().FirstOrDefault();
            
            var product = Mapper.Map<ProductViewModel, Product>(productVm);

            if (oldProduct != null) product.DateCreated = oldProduct.DateCreated;

            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public ProductViewModel GetProductById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_unitOfWork.ProductRepository.FindById(id));
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                Product product;
                for(int i= worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = worksheet.Cells[i, 1].Value.ToString();
                    product.Description = worksheet.Cells[i, 2].Value.ToString();

                    decimal.TryParse(worksheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;

                    decimal.TryParse(worksheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;

                    decimal.TryParse(worksheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    product.PromotionPrice = promotionPrice;

                    product.Content = worksheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = worksheet.Cells[i, 7].Value.ToString();
                    product.SeoDescription = worksheet.Cells[i, 8].Value.ToString();

                    bool.TryParse(worksheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;

                    bool.TryParse(worksheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    _unitOfWork.ProductRepository.Add(product);
                    _unitOfWork.Commit();
                }
            }
           
        }
    }
}
