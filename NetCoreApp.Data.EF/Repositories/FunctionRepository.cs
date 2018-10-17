using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class FunctionRepository : EfRepository<Function, string>, IFunctionRepository
    {
        private AppDbContext _context;

        public FunctionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
