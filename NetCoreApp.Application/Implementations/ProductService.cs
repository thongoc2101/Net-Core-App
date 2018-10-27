﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Utilities.Dtos;

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
    }
}
