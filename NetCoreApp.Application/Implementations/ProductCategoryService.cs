using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.Enums;

namespace NetCoreApp.Application.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _unitOfWork.ProductCategoryRepository.Add(productCategory);
            _unitOfWork.Commit();
            return productCategoryVm;
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _unitOfWork.ProductCategoryRepository.Update(productCategory);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductCategoryRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _unitOfWork.ProductCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return _unitOfWork.ProductCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                                                                         || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();

            }
            else
            {
                return _unitOfWork.ProductCategoryRepository.FindAll()
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
            }
            
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _unitOfWork.ProductCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_unitOfWork.ProductCategoryRepository.FindById(id));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _unitOfWork.ProductCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _unitOfWork.ProductCategoryRepository.Update(sourceCategory);

            //Get all sibling ( lay ho hang anh em ra)
            var sibling = _unitOfWork.ProductCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _unitOfWork.ProductCategoryRepository.Update(child);
            }
            _unitOfWork.Commit();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _unitOfWork.ProductCategoryRepository.FindById(sourceId);
            var target = _unitOfWork.ProductCategoryRepository.FindById(targetId);

            var tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _unitOfWork.ProductCategoryRepository.Update(source);
            _unitOfWork.ProductCategoryRepository.Update(target);
            _unitOfWork.Commit();
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _unitOfWork.ProductCategoryRepository.FindAll(x => x.HomeFlag == true
                                                           ,c => c.Products)
                .OrderBy(x => x.HomeOrder).Take(top).ProjectTo<ProductCategoryViewModel>().ToList();
            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Prod
            }

            return categories;
        }
    }
}
