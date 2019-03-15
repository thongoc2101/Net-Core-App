using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel roleViewModel);

        Task DeleteAsync(Guid id);

        Task<List<AppRoleViewModel>> GetAllAsync();

        PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetByIdAsync(Guid id);

        Task UpdateAsync(AppRoleViewModel roleViewModel);

        List<PermissionViewModel> GetListFunctionWithRoles(Guid roleId);
        
        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
