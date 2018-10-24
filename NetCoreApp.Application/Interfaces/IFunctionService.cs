using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreApp.Application.ViewModels;

namespace NetCoreApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task<List<FunctionViewModel>> GetAll();

        List<FunctionViewModel> GetAllByPermission(Guid userId);

    }
}
