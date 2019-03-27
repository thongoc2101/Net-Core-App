using NetCoreApp.Data.Entities;
using NetCoreApp.Infrastructure.Interfaces;

namespace NetCoreApp.Data.IRepositories
{
    public interface IProductImageRepository: IRepository<ProductImage, int>
    {
    }
}
