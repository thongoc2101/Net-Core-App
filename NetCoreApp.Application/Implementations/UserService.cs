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
using NetCoreApp.Data.Entities;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(AppUserViewModel userViewModel)
        {
            var user = new AppUser()
            {
                UserName = userViewModel.UserName,
                Avatar = userViewModel.Avatar,
                Email = userViewModel.Email,
                FullName = userViewModel.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userViewModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded && userViewModel.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.FullName);
                if (appUser != null)
                {
                    await _userManager.AddToRolesAsync(appUser, userViewModel.Roles);
                }
            }

            return true;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x =>
                    x.FullName.Contains(keyword) || x.Email.Contains(keyword) || x.UserName.Contains(keyword));
            
            int totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated
            }).ToList();

            var paging = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paging;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = Mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }

        public async Task UpdateAsync(AppUserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString());

            // Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user); // get danh sach roles hien tai

            var result = await _userManager.AddToRolesAsync(user, userViewModel.Roles.Except(currentRoles).ToArray()); // add tat ca roles tru roles hien tai
            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userViewModel.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                // Update user detail
                user.FullName = userViewModel.FullName;
                user.Status = userViewModel.Status;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
