using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Data.Interfaces
{
    public interface IHasSeoMetaData
    {
        string SeoPageTitle { set; get; }

        string SeoAlias { set; get; }

        string SeoKeywords { set; get; }

        string SeoDescription { get; set; }
    }
}
