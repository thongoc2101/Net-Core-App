using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Infrastructure.Interfaces;

namespace NetCoreApp.Data.IRepositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
