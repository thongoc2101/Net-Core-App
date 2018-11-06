using System;
using System.Collections.Generic;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel Add(ProductViewModel productViewModel);

        void Update(ProductViewModel productViewModel);

        void Delete(int id);

        ProductViewModel GetProductById(int id);

        void ImportExcel(string filePath, int categoryId);
    }
}
