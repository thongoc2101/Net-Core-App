using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public string BodyClass { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }

        public PagedResult<ProductViewModel> Data { set; get; }

        public ProductCategoryViewModel Category { set; get; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "lastest", Text = "Lastest"},
            new SelectListItem(){Value = "price", Text = "Price"},
            new SelectListItem(){Value = "name", Text = "Name"}
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12", Text = "12"},
            new SelectListItem(){Value = "24", Text = "24"},
            new SelectListItem(){Value = "48", Text = "48"}
        };
    }
}
