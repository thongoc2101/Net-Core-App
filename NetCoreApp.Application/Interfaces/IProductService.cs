using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();
    }
}
