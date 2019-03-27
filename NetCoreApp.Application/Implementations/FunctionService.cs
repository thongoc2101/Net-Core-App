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
        //private readonly IMapper _mapper;

        public FunctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
        }

        public void Add(FunctionViewModel functionViewModel)
        {
            //var function = _mapper.Map<Function>(functionViewModel);
            var function = Mapper.Map<FunctionViewModel, Function>(functionViewModel);
            _unitOfWork.FunctionRepository.Add(function);
            _unitOfWork.Commit();
        }

        public bool CheckExistedId(string id)
        {
            return _unitOfWork.FunctionRepository.FindById(id) != null;
        }

        public void Delete(string id)
        {
            _unitOfWork.FunctionRepository.Remove(id);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _unitOfWork.FunctionRepository.FindAll(x=> x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter) || x.Id.Contains(filter));
            }

            return query.OrderBy(x=> x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _unitOfWork.FunctionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }

        public FunctionViewModel GetById(string id)
        {
            return Mapper.Map<Function, FunctionViewModel>(_unitOfWork.FunctionRepository.FindSingle(x=> x.Id == id));
        }

        public void ReOrder(string sourceId, string targetId)
        {
            var source = _unitOfWork.FunctionRepository.FindById(sourceId);
            var target = _unitOfWork.FunctionRepository.FindById(targetId);

            var tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _unitOfWork.FunctionRepository.Update(source);
            _unitOfWork.FunctionRepository.Update(target);

            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionViewModel)
        {
            var functionDb = _unitOfWork.FunctionRepository.FindById(functionViewModel.Id);
            var function = Mapper.Map<Function>(functionViewModel);

            _unitOfWork.Commit();
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            var sourceCategory = _unitOfWork.FunctionRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _unitOfWork.FunctionRepository.Update(sourceCategory);

            //Get all sibling ( lay ho hang anh em ra)
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
