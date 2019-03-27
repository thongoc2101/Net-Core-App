using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;
using NetCoreApp.Infrastructure.Interfaces;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddAsync(AppRoleViewModel roleViewModel)
        {
            var role = new AppRole()
            {
                Name = roleViewModel.Name,
                Description = roleViewModel.Description
            };
            var result = await _roleManager.CreateAsync(role);

            _unitOfWork.Commit();
            return result.Succeeded;

        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _unitOfWork.FunctionRepository.FindAll();
            var permissions = _unitOfWork.PermissionRepository.FindAll();

            var query = from f in functions
                join p in permissions on f.Id equals p.FunctionId
                join r in _roleManager.Roles on p.RoleId equals r.Id
                where roles.Contains(r.Name) && f.Id == functionId &&
                      ((p.CanRead && action == "Read") ||
                       (p.CanCreate && action == "Create")
                       || (p.CanDelete && action == "Delete") ||
                       (p.CanUpdate && action == "Update"))
                select p;

            _unitOfWork.Commit();
            return query.AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
            _unitOfWork.Commit();
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x =>
                    x.Name.Contains(keyword) || x.Description.Contains(keyword));
            
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<AppRoleViewModel>().ToList();
            var paging = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page, 
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paging;
        }

        public async Task<AppRoleViewModel> GetByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return Mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public List<PermissionViewModel> GetListFunctionWithRoles(Guid roleId)
        {
            var functions = _unitOfWork.FunctionRepository.FindAll();
            var permissions = _unitOfWork.PermissionRepository.FindAll();

            var query = from f in functions
                join p in permissions on f.Id equals p.FunctionId into fp
                from p in fp.DefaultIfEmpty()
                where p != null && p.RoleId == roleId
                select new PermissionViewModel()
                {
                    RoleId = roleId,
                    FunctionId = f.Id,
                    CanCreate = p != null && p.CanCreate,
                    CanDelete = p != null && p.CanDelete,
                    CanRead = p != null && p.CanRead,
                    CanUpdate = p != null && p.CanUpdate
                };

            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVm, Guid roleId)
        {
            var permissions = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVm);
            var oldPermission = _unitOfWork.PermissionRepository.FindAll(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _unitOfWork.PermissionRepository.RemoveMultiple(oldPermission);
            }

            foreach (var permission in permissions)
            {
                _unitOfWork.PermissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id.ToString());
            role.Description = roleViewModel.Description;
            role.Name = roleViewModel.Name;
            await _roleManager.UpdateAsync(role);
            _unitOfWork.Commit();
        }
    }
}
