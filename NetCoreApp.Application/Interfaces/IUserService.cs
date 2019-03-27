using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Dtos;

// using System.Threading.Tasks; support dang bat dong bo async await
namespace NetCoreApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userViewModel);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);

        Task UpdateAsync(AppUserViewModel userViewModel);
    }
}
