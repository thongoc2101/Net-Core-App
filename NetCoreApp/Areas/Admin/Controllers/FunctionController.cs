using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllFilter(string filter)
        {
            var model = ServiceRegistration.FunctionService.GetAll(filter);
            return new ObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await ServiceRegistration.FunctionService.GetAll(string.Empty);
            var rootFunctions = model.Where(c => c.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }

        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = ServiceRegistration.FunctionService.GetAll(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel functionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(functionVm.Id))
                {
                    ServiceRegistration.FunctionService.Add(functionVm);
                }
                else
                {
                    ServiceRegistration.FunctionService.Update(functionVm);
                }

                return new OkObjectResult(functionVm);
            }
        }

        [HttpPost]
        public IActionResult UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    ServiceRegistration.FunctionService.UpdateParentId(sourceId, targetId, items);
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult ReOrder(string sourceId, string targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    ServiceRegistration.FunctionService.ReOrder(sourceId, targetId);
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            else
            {
                ServiceRegistration.FunctionService.Delete(id);
                return new OkObjectResult(id);
            }
        }

        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.Id);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
    }
}