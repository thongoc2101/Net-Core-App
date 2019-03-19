using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel GetProductById(int id);

        ProductViewModel Add(ProductViewModel productVm);

        void Update(ProductViewModel productVm);

        void Delete(int id);

        // filePath la url len service
        void ImportExcel(string filePath, int categoryId);

        void AddQuantities(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetHotProduct(int top);

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpSellProducts(int top);

        List<TagViewModel> GetTags(int productId);

        bool CheckAvailable(int productId, int size, int color);
    }
}
