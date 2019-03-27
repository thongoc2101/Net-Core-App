using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { set; get; }

        public List<ProductViewModel> RelatedProduct { set; get; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImage { get; set; }

        public List<ProductViewModel> UpSellProduct { set; get; }

        public List<ProductViewModel> LastestProduct { set; get; }

        public List<TagViewModel> Tags { get; set; }

        public List<SelectListItem> Colors { set; get; }

        public List<SelectListItem> Sizes { set; get; }

        public bool Available { set; get; }
    }
}
