using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new ObjectResult(model);
        }

        /// <summary>
        /// get by id role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetByIdAsync(id);
            return new OkObjectResult(model);
        }

        /// <summary>
        /// get list all function
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRoles(roleId);
            return new ObjectResult(functions);
        }

        /// <summary>
        /// Save permission
        /// </summary>
        /// <param name="listPermission"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            _roleService.SavePermission(listPermission, roleId);
            return new OkResult();
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }

        /// <summary>
        /// get all paging role
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        /// <summary>
        /// save update or add role
        /// </summary>
        /// <param name="roleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (roleViewModel.Id == null)
                {
                    await _roleService.AddAsync(roleViewModel);
                }
                else
                {
                    await _roleService.UpdateAsync(roleViewModel);
                }

                return new ObjectResult(roleViewModel);
            }
        }
    }
}