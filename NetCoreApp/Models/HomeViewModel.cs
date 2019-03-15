using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Models
{
    public class HomeViewModel
    {
        public List<BlogViewModel> LastestBlogs { get; set; }

        public List<SlideViewModel> HomeSlides { get; set; }

        public List<ProductViewModel> HotProducts { get; set; }

        public List<ProductViewModel> TopSellProducts { get; set; }

        public List<ProductCategoryViewModel> HomeCategories { get; set; }

        public string Title { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }
    }
}
