using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.Enums;

namespace NetCoreApp.Application.Implementations
{
    public class FunctionService : IFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FunctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool CheckExistedId(string id)
        {
            return _unitOfWork.FunctionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = Mapper.Map<FunctionViewModel, Function>(functionVm);
            _unitOfWork.FunctionRepository.Add(function);
            _unitOfWork.Commit();
        }

        public void Delete(string id)
        {
            _unitOfWork.FunctionRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _unitOfWork.FunctionRepository.FindSingle(x => x.Id == id);
            return Mapper.Map<Function, FunctionViewModel>(function);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _unitOfWork.FunctionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));
            return query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _unitOfWork.FunctionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }

        public void Update(FunctionViewModel functionVm)
        {

            var functionDb = _unitOfWork.FunctionRepository.FindById(functionVm.Id);
            var function = Mapper.Map<FunctionViewModel, Function>(functionVm);

            _unitOfWork.Commit();
        }

        public void ReOrder(string sourceId, string targetId)
        {

            var source = _unitOfWork.FunctionRepository.FindById(sourceId);
            var target = _unitOfWork.FunctionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _unitOfWork.FunctionRepository.Update(source);
            _unitOfWork.FunctionRepository.Update(target);

            _unitOfWork.Commit();

        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _unitOfWork.FunctionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _unitOfWork.FunctionRepository.Update(category);

            //Get all sibling
            var sibling = _unitOfWork.FunctionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _unitOfWork.FunctionRepository.Update(child);
            }

            _unitOfWork.Commit();
        }
    }
}
